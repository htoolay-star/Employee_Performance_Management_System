using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;
using EPMS.Domain.Interface.IService.Performance;

using Mapster;
namespace EPMS.Domain.Services.Performance;

public class AppraisalService : IAppraisalService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _timeProvider;
        private readonly ICurrentEmployeeContextService _currentEmployee;

    public AppraisalService(
        IUnitOfWork uow,
        TimeProvider timeProvider,
        ICurrentEmployeeContextService currentEmployee)
    {
        _uow = uow;
        _timeProvider = timeProvider;
        _currentEmployee = currentEmployee;
    }

    public async Task<SuccessResponse> CreateAsync(CreateAppraisalDto dto)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(dto.CycleId);
        if (cycle == null)
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(dto.CycleId), ErrorType.NotFound);

        var now = _timeProvider.GetUtcNow();
        var today = DateOnly.FromDateTime(now.DateTime);

        if (today < cycle.EvaluationStartDate)
            return SuccessResponse.Fail("Evaluation period hasn't started yet.", ErrorType.Validation);

        if (today > cycle.WindowEndDate)
            return SuccessResponse.Fail("The appraisal window has ended.", ErrorType.Validation);

        if (cycle.IsLocked)
            return SuccessResponse.Fail("Cycle is locked.", ErrorType.Validation);

        if (dto.EmployeeId.HasValue)
        {
            var employee = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId.Value);
            if (employee == null)
                return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(dto.EmployeeId.Value), ErrorType.NotFound);

            var hasExisting = await _uow.Perf.Appraisals.ExistsByEmployeeAndCycleAsync(dto.EmployeeId.Value, dto.CycleId);
            if (hasExisting)
                return SuccessResponse.Fail(AppraisalMsg.DuplicateEntry, ErrorType.Conflict);
        }
        else if (!string.IsNullOrEmpty(dto.EntityType) && dto.EntityId.HasValue)
        {
            var hasExisting = await _uow.Perf.Appraisals.ExistsByEntityAndCycleAsync(dto.EntityType, dto.EntityId.Value, dto.CycleId);
            if (hasExisting)
                return SuccessResponse.Fail(AppraisalMsg.DuplicateEntry, ErrorType.Conflict);
        }
        else
        {
            return SuccessResponse.Fail("Either EmployeeId or EntityType+EntityId must be provided.", ErrorType.Validation);
        }

        var managerReviewer = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.ManagerReviewerId);
        if (managerReviewer == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(dto.ManagerReviewerId), ErrorType.NotFound);

        Appraisal appraisal;
        if (dto.EmployeeId.HasValue)
        {
            appraisal = new Appraisal(dto.EmployeeId.Value, dto.CycleId, dto.ManagerReviewerId);
        }
        else
        {
            appraisal = new Appraisal(dto.EntityType!, dto.EntityId!.Value, dto.CycleId, dto.ManagerReviewerId);
        }

        await ResolveAndAddKPIDetailsAsync(appraisal, dto.CycleId);

        _uow.Perf.Appraisals.Add(appraisal);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(AppraisalMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateAppraisalDto dto)
    {
        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        appraisal.UpdateDetails(dto.Status, dto.EmployeeComment, dto.ManagerComment, dto.RatingLabel);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        appraisal.IsDeleted = true;
        appraisal.DeletedAt = _timeProvider.GetUtcNow();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalMsg.Deleted);
    }

    public async Task<SuccessResponse> GetByIdAsync(long id)
    {
        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        var dto = appraisal.Adapt<AppraisalDto>();
        dto = await ResolveEntityNameAsync(dto);
        return SuccessResponse<AppraisalDto>.Ok(dto, AppraisalMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAllAsync()
    {
        var appraisals = await _uow.Perf.Appraisals.GetAllAsync();
        var dtos = new List<AppraisalDto>();
        foreach (var appraisal in appraisals.Where(a => !a.IsDeleted))
        {
            var dto = MapToDto(appraisal);
            dto = await ResolveEntityNameAsync(dto);
            dtos.Add(dto);
        }
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByEmployeeIdAsync(long employeeId)
    {
        var employee = await _uow.Info.EmployeeProfiles.GetByIdAsync(employeeId);
        if (employee == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(employeeId), ErrorType.NotFound);

        var appraisals = await _uow.Perf.Appraisals.GetEmployeeAppraisalsAsync(employeeId, 0);
        var dtos = new List<AppraisalDto>();
        foreach (var appraisal in appraisals.Where(a => !a.IsDeleted))
        {
            var dto = MapToDto(appraisal);
            dto = await ResolveEntityNameAsync(dto);
            dtos.Add(dto);
        }
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedByEmployee);
    }

    public async Task<SuccessResponse> GetMyEvaluationsAsync()
    {
        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue)
            return SuccessResponse.Fail("User identity not found.", ErrorType.Forbidden);

        var appraisals = await _uow.Perf.Appraisals.GetByManagerReviewerIdAsync(currentEmployeeId.Value);
        var dtos = appraisals.Select(MapToDto).ToList();
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByEntityTypeAndCycleAsync(string entityType, long cycleId)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(cycleId);
        if (cycle == null)
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(cycleId), ErrorType.NotFound);

        var appraisals = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.EntityType == entityType && a.CycleId == cycleId && !a.IsDeleted,
            includes: a => a.Details);

        var dtos = (await Task.WhenAll(
            appraisals.Select(a => ResolveEntityNameAsync(MapToDto(a)))))
            .AsEnumerable();
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> UpdateDetailActualValuesAsync(long appraisalId, List<AppraisalDetailDto> details)
    {
        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(appraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        foreach (var dto in details)
        {
            var detail = appraisal.Details.FirstOrDefault(d => d.KPIId == dto.KPIId);
            if (detail == null) continue;
            detail.Evaluate(dto.ActualValue, dto.Comment);
        }

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalMsg.Updated);
    }

    private static AppraisalDto MapToDto(Appraisal appraisal)
    {
        return new AppraisalDto(
            Id: appraisal.Id,
            EmployeeId: appraisal.EmployeeId,
            EmployeeName: appraisal.Employee?.StaffName,
            EntityType: appraisal.EntityType,
            EntityId: appraisal.EntityId,
            EntityName: null,
            CycleId: appraisal.CycleId,
            CycleName: appraisal.Cycle?.Name,
            ManagerReviewerId: appraisal.ManagerReviewerId,
            ManagerReviewerName: appraisal.ManagerReviewer?.StaffName,
            Status: appraisal.Status ?? "Draft",
            RatingLabel: appraisal.RatingLabel,
            TotalScore: appraisal.TotalScore,
            EmployeeComment: appraisal.EmployeeComment,
            ManagerComment: appraisal.ManagerComment,
            ReviewDate: appraisal.ReviewDate,
            IsLocked: appraisal.IsLocked,
            LockedAt: appraisal.LockedAt,
            FinalizedDate: appraisal.FinalizedDate,
            CreatedAt: appraisal.CreatedAt
        );
    }

    private async Task<AppraisalDto> ResolveEntityNameAsync(AppraisalDto dto)
    {
        if (string.IsNullOrEmpty(dto.EntityType) || !dto.EntityId.HasValue)
            return dto;

        string? entityName = dto.EntityType switch
        {
            AppraisalConstants.EntityTypes.Department => (await _uow.HR.Departments.GetByIdAsync(dto.EntityId.Value))?.Name,
            AppraisalConstants.EntityTypes.Team => (await _uow.HR.Teams.GetByIdAsync(dto.EntityId.Value))?.Name,
            _ => null
        };

        return dto with { EntityName = entityName };
    }

    public async Task<SuccessResponse> GetAppraisalFillAsync(long id)
    {
        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue || currentEmployeeId.Value != appraisal.ManagerReviewerId)
            return SuccessResponse.Fail("Only the manager reviewer can view KPI evaluation.", ErrorType.Forbidden);

            var dto = new AppraisalFillDto
            {
                Id = appraisal.Id,
                EmployeeId = appraisal.EmployeeId ?? 0,
                EmployeeName = appraisal.Employee?.StaffName,
                StaffNo = appraisal.Employee?.StaffNo ?? string.Empty,
                PositionName = appraisal.Employee?.Employment?.Position?.Name,
                DepartmentName = appraisal.Employee?.Employment?.Department?.Name,
                TeamName = appraisal.Employee?.Employment?.Team?.Name,
                ManagerName = appraisal.Employee?.Employment?.DirectManager?.StaffName,
                CycleId = appraisal.CycleId,
                CycleName = appraisal.Cycle?.Name,
                ManagerReviewerId = appraisal.ManagerReviewerId,
                ManagerReviewerName = appraisal.ManagerReviewer?.StaffName,
                Status = appraisal.Status,
                IsLocked = appraisal.IsLocked,
                Details = appraisal.Details.Select(d => new AppraisalDetailFillDto
                {
                    KPIId = d.KPIId,
                    KPIName = d.KPIName,
                    CategoryName = d.CategoryName,
                    Weightage = d.Weightage,
                    TargetValue = d.TargetValue,
                    ScoringDirection = d.ScoringDirection,
                    ActualValue = d.ActualValue,
                    Score = d.Score,
                    WeightedScore = d.WeightedScore,
                    Remarks = d.Remarks,
                }).ToList()
            };

        return SuccessResponse<AppraisalFillDto>.Ok(dto, AppraisalMsg.Retrieved);
    }

    public async Task<SuccessResponse> SubmitAsync(AppraisalSubmissionDto dto)
    {
        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue)
            return SuccessResponse.Fail("User identity not found.", ErrorType.Forbidden);

        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(dto.Id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(dto.Id), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        if (currentEmployeeId != appraisal.ManagerReviewerId)
            return SuccessResponse.Fail("You are not authorized to submit this appraisal.", ErrorType.Forbidden);

        if (appraisal.Cycle != null)
        {
            var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
            var start = appraisal.Cycle.ManagerReviewStartDate ?? appraisal.Cycle.WindowStartDate;
            var deadline = appraisal.Cycle.ManagerReviewDeadline ?? appraisal.Cycle.WindowEndDate;

            if (today < start)
                return SuccessResponse.Fail($"Appraisal review window opens on {start:dd/MM/yyyy}.", ErrorType.Validation);
            if (today > deadline)
                return SuccessResponse.Fail($"Appraisal review window closed on {deadline:dd/MM/yyyy}.", ErrorType.Validation);
        }

        foreach (var detailDto in dto.Details)
        {
            var detail = appraisal.Details.FirstOrDefault(d =>
                d.KPIId == detailDto.KPIId);

            if (detail != null)
            {
                detail.Evaluate(detailDto.ActualValue, detailDto.Comment);
            }
        }

        _uow.Perf.Appraisals.Update(appraisal);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(AppraisalMsg.Submitted);
    }

    public async Task<SuccessResponse> LockAsync(long id, long adminId, string reason)
    {
        var positionId = await _currentEmployee.GetPositionIdAsync();
        if (!positionId.HasValue)
            return SuccessResponse.Fail("User position is required.", ErrorType.Forbidden);

        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        appraisal.Lock(_timeProvider);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalMsg.Locked);
    }

    public async Task<SuccessResponse> UnlockAsync(long id, long adminId, string reason)
    {
        var positionId = await _currentEmployee.GetPositionIdAsync();
        if (!positionId.HasValue)
            return SuccessResponse.Fail("User position is required.", ErrorType.Forbidden);

        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        if (!appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyUnlocked, ErrorType.Conflict);

        if (string.IsNullOrWhiteSpace(reason))
            return SuccessResponse.Fail(AppraisalMsg.UnlockReasonRequired, ErrorType.Validation);

        appraisal.UnlockAppraisal(adminId, reason, _timeProvider);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalMsg.Unlocked);
    }

    public async Task<SuccessResponse> UnlockRoleAsync(long id, string role)
    {
        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        switch (role)
        {
            case EvaluatorRoles.Self:
                if (appraisal.SelfLockIsDeadline)
                    return SuccessResponse.Fail("Cannot unlock deadline-locked self evaluation.", ErrorType.Conflict);
                appraisal.UnlockSelf();
                break;
            case EvaluatorRoles.Manager:
            case EvaluatorRoles.Peer:
            case EvaluatorRoles.Subordinate:
                if (appraisal.ThreeSixtyLockIsDeadline)
                    return SuccessResponse.Fail("Cannot unlock deadline-locked 360 evaluation.", ErrorType.Conflict);
                appraisal.UnlockThreeSixty();
                break;
            case EvaluatorRoles.Appraisal:
                if (appraisal.AppraisalLockIsDeadline)
                    return SuccessResponse.Fail("Cannot unlock deadline-locked appraisal evaluation.", ErrorType.Conflict);
                appraisal.UnlockAppraisalLock();
                break;
            default:
                return SuccessResponse.Fail($"Unknown role '{role}'.", ErrorType.Validation);
        }

        var responses = await _uow.Perf.EvaluationResponses
            .FindAllAsync(r => r.AppraisalId == id && r.EvaluatorRole == role && !r.IsDeleted,
                          trackChanges: true);
        foreach (var r in responses)
            r.ClearSubmission();

        _uow.Perf.Appraisals.Update(appraisal);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok($"{role} evaluation unlocked successfully.");
    }

    public async Task AutoGenerateForCycleAsync(long cycleId)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(cycleId);
        if (cycle == null || cycle.IsLocked) return;

        await GenerateAppraisalsCoreAsync(cycleId, cycle);
    }

    private async Task<(int Created, int Skipped)> GenerateAppraisalsCoreAsync(long cycleId, AppraisalCycle cycle)
    {
        var created = 0;
        var skipped = 0;

        // 1. Collect employees from EmployeeKPI (per-employee assignments)
        var employeeKpiRecords = await _uow.Perf.EmployeeKPIs
            .FindAllAsync(k => k.CycleId == cycleId && !k.IsDeleted);
        var employeeIds = new HashSet<long>(employeeKpiRecords.Select(k => k.EmployeeId));

        // 2. Collect employees from Position EntityKPI
        var positionKPIs = await _uow.Perf.EntityKPIs
            .GetByEntityTypeAsync(AppraisalConstants.EntityTypes.Position);
        var positionIds = positionKPIs.Where(e => !e.IsDeleted)
            .Select(e => e.EntityId).Distinct().ToList();

        if (positionIds.Count != 0)
        {
            var employments = await _uow.Info.EmployeeEmployments
                .FindAllAsync(e => positionIds.Contains(e.PositionId));
            foreach (var emp in employments)
                employeeIds.Add(emp.EmployeeId);
        }

        // 3. Process each employee
        foreach (var employeeId in employeeIds)
        {
            var exists = await _uow.Perf.Appraisals.ExistsByEmployeeAndCycleAsync(employeeId, cycleId);
            if (exists) { skipped++; continue; }

            var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(employeeId);
            if (employment == null) { skipped++; continue; }

            if (employment.EmploymentStatus != EmploymentStatuses.Permanent)
            {
                if (!employment.DateOfAppointment.HasValue || employment.DateOfAppointment.Value > cycle.EvaluationStartDate)
                {
                    skipped++; continue;
                }
            }

            if (employment.DateOfTermination.HasValue && employment.DateOfTermination.Value <= cycle.EvaluationEndDate)
            {
                skipped++; continue;
            }

            if (employment.DirectManagerId == null) { skipped++; continue; }

            var managerReviewerId = employment.DirectManagerId.Value;
            var appraisal = new Appraisal(employeeId, cycleId, managerReviewerId);

            await ResolveAndAddKPIDetailsAsync(appraisal, cycleId);
            _uow.Perf.Appraisals.Add(appraisal);

            var positionTemplates = await _uow.Perf.PositionFormTemplates
                .GetByPositionIdWithQuestionsAsync(employment.PositionId);

            foreach (var pt in positionTemplates)
            {
                var template = pt.FormTemplate;
                if (template?.Questions == null || !template.IsActive) continue;

                var entries = await ResolveEvaluatorEntriesAsync(employment, template.FormType, employeeId, managerReviewerId, cycle);

                foreach (var (evaluatorId, role) in entries)
                {
                    var pool = template.Questions.ToList();
                    var take = template.QuestionsPerEvaluation.GetValueOrDefault(pool.Count);
                    var selected = pool.OrderBy(_ => Guid.NewGuid()).Take(take).ToList();

                    foreach (var question in selected)
                    {
                        var response = new EvaluationResponse(
                            appraisal.Id, template.Id, question.Id,
                            evaluatorId, role, isAnonymous: role is EvaluatorRoles.Peer or EvaluatorRoles.Subordinate or EvaluatorRoles.Manager);
                        appraisal.AddResponse(response);
                    }
                }
            }

            await _uow.CompleteAsync();
            created++;
        }

        // 4. Generate entity appraisals for Departments with EntityKPIs
        var departmentKPIs = (await _uow.Perf.EntityKPIs
            .GetByEntityTypeAsync(AppraisalConstants.EntityTypes.Department))
            .Where(e => !e.IsDeleted)
            .ToList();

        var departmentIds = departmentKPIs
            .Select(e => e.EntityId).Distinct().ToList();

        foreach (var deptId in departmentIds)
        {
            var exists = await _uow.Perf.Appraisals.ExistsByEntityAndCycleAsync(
                AppraisalConstants.EntityTypes.Department, deptId, cycleId);
            if (exists) { skipped++; continue; }

            var department = await _uow.HR.Departments.GetByIdAsync(deptId);
            if (department == null) { skipped++; continue; }

            var managerId = department.DeptHeadId ?? await GetDefaultReviewerIdAsync();
            if (managerId == null) { skipped++; continue; }

            var appraisal = new Appraisal(
                AppraisalConstants.EntityTypes.Department, deptId, cycleId, managerId.Value);

            await ResolveAndAddKPIDetailsAsync(appraisal, cycleId);

            _uow.Perf.Appraisals.Add(appraisal);
            await _uow.CompleteAsync();
            created++;
        }

        // 5. Generate entity appraisals for Teams with EntityKPIs
        var teamKPIs = (await _uow.Perf.EntityKPIs
            .GetByEntityTypeAsync(AppraisalConstants.EntityTypes.Team))
            .Where(e => !e.IsDeleted)
            .ToList();

        var teamIds = teamKPIs
            .Select(e => e.EntityId).Distinct().ToList();

        foreach (var teamId in teamIds)
        {
            var exists = await _uow.Perf.Appraisals.ExistsByEntityAndCycleAsync(
                AppraisalConstants.EntityTypes.Team, teamId, cycleId);
            if (exists) { skipped++; continue; }

            var team = await _uow.HR.Teams.GetByIdAsync(teamId);
            if (team == null) { skipped++; continue; }

            var managerId = team.LeadTeamId ?? await GetDefaultReviewerIdAsync();
            if (managerId == null) { skipped++; continue; }

            var appraisal = new Appraisal(
                AppraisalConstants.EntityTypes.Team, teamId, cycleId, managerId.Value);

            await ResolveAndAddKPIDetailsAsync(appraisal, cycleId);

            _uow.Perf.Appraisals.Add(appraisal);
            await _uow.CompleteAsync();
            created++;
        }

        return (created, skipped);
    }

    private async Task<long?> GetDefaultReviewerIdAsync()
    {
        var adminUser = await _uow.Auth.Users.FindAsync(
            u => u.RoleId == (long)UserRole.Admin && !u.IsDeleted,
            includes: u => u.Profile);
        if (adminUser?.Profile != null)
            return adminUser.Profile.Id;
        var systemAdmin = await _uow.Auth.Users.FindAsync(
            u => u.RoleId == (long)UserRole.SystemAdmin && !u.IsDeleted,
            includes: u => u.Profile);
        return systemAdmin?.Profile?.Id;
    }

    public async Task<SuccessResponse> FinalizeAsync(long id)
    {
        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        var kpiScore = appraisal.Details
            .Where(d => d.KPIId.HasValue && d.Score > 0)
            .Select(d => d.WeightedScore)
            .DefaultIfEmpty(0)
            .Average();

        var responses = await _uow.Perf.EvaluationResponses
            .FindAllAsync(r => r.AppraisalId == id && !r.IsDeleted,
                includes: r => r.Question);

        // All questions share the same rating scale from the form template
        var anyResponse = responses.FirstOrDefault(r => r.TemplateId != 0);
        var maxScale = 5m;
        if (anyResponse != null)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(anyResponse.TemplateId);
            if (template != null)
            {
                var scaleWithLevels = await _uow.Perf.QuestionRatingScales.FindAsync(
                    s => s.Id == template.QuestionRatingScaleId,
                    includes: s => s.Levels);
                maxScale = scaleWithLevels?.Levels.Any() == true
                    ? Math.Max(scaleWithLevels.Levels.Max(l => l.Rating), 1m)
                    : 5m;
            }
        }

        var selfScore = responses
            .Where(r => r.EvaluatorRole == EvaluatorRoles.Self && r.RatingValue.HasValue)
            .Select(r => (decimal)r.RatingValue!.Value)
            .DefaultIfEmpty(0)
            .Average() * 100m / maxScale;

        // 360 includes Manager + Peer + Subordinate
        var threeSixtyScore = responses
            .Where(r => (r.EvaluatorRole is EvaluatorRoles.Manager or EvaluatorRoles.Peer or EvaluatorRoles.Subordinate) && r.RatingValue.HasValue)
            .Select(r => (decimal)r.RatingValue!.Value)
            .DefaultIfEmpty(0)
            .Average() * 100m / maxScale;

        var appraisalScore = responses
            .Where(r => r.EvaluatorRole == EvaluatorRoles.Appraisal && r.RatingValue.HasValue)
            .Select(r => (decimal)r.RatingValue!.Value)
            .DefaultIfEmpty(0)
            .Average() * 100m / maxScale;

        if (appraisal.Cycle == null)
            return SuccessResponse.Fail("Cycle data not loaded.", ErrorType.NotFound);

        var ratingScales = await _uow.Perf.RatingScales
            .FindAllAsync(s => s.IsActive && !s.IsDeleted);

        var totalBeforeMatch = (kpiScore * appraisal.Cycle.KpiWeight / 100m)
                             + (selfScore * appraisal.Cycle.SelfWeight / 100m)
                             + (threeSixtyScore * appraisal.Cycle.ThreeSixtyWeight / 100m)
                             + (appraisalScore * appraisal.Cycle.AppraisalWeight / 100m);

        var matchingScale = ratingScales.FirstOrDefault(s => s.IsMatch(totalBeforeMatch));
        if (matchingScale == null)
            return SuccessResponse.Fail("No matching rating scale found.", ErrorType.Validation);

        appraisal.FinalizeAppraisal(
            kpiScore, selfScore, threeSixtyScore, appraisalScore,
            appraisal.Cycle.KpiWeight, appraisal.Cycle.SelfWeight,
            appraisal.Cycle.ThreeSixtyWeight,
            appraisal.Cycle.AppraisalWeight,
            matchingScale, _timeProvider);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalMsg.Locked);
    }

    private async Task<List<(long EvaluatorId, string Role)>> ResolveEvaluatorEntriesAsync(
        Entities.EmployeeInfo.EmployeeEmployment employment, string formType,
        long employeeId, long managerReviewerId, AppraisalCycle? cycle)
    {
        var entries = new List<(long, string)>();

        switch (formType)
        {
            case AppraisalConstants.FormTypes.Self:
                entries.Add((employeeId, EvaluatorRoles.Self));
                break;

            case AppraisalConstants.FormTypes.Manager:
                entries.Add((managerReviewerId, EvaluatorRoles.Manager));
                break;

            case AppraisalConstants.FormTypes.Peer:
                var peers = await SelectRandomPeersAsync(employment.PositionId, employeeId, 3, 5);
                entries.AddRange(peers.Select(id => (id, EvaluatorRoles.Peer)));
                break;

            case AppraisalConstants.FormTypes.Subordinate:
                var subordinates = await SelectRandomSubordinatesAsync(employeeId, 5);
                entries.AddRange(subordinates.Select(id => (id, EvaluatorRoles.Subordinate)));
                break;

            case AppraisalConstants.FormTypes.Appraisal:
                entries.Add((managerReviewerId, EvaluatorRoles.Appraisal));
                break;
        }

        return entries;
    }

    private async Task<List<long>> SelectRandomPeersAsync(long positionId, long excludeEmployeeId, int min, int max)
    {
        var peers = await _uow.Info.EmployeeEmployments
            .FindAllAsync(e => e.PositionId == positionId && e.EmployeeId != excludeEmployeeId && !e.IsDeleted);
        var peerList = peers.Select(e => e.EmployeeId).Distinct().ToList();
        var count = Math.Clamp(peerList.Count, 0, max);
        return peerList.OrderBy(_ => Guid.NewGuid()).Take(Math.Max(count, min)).ToList();
    }

    private async Task<List<long>> SelectRandomSubordinatesAsync(long employeeId, int max)
    {
        var subordinates = await _uow.Info.EmployeeEmployments
            .FindAllAsync(e => e.DirectManagerId == employeeId && !e.IsDeleted);
        var subList = subordinates.Select(e => e.EmployeeId).Distinct().ToList();
        var count = Math.Min(subList.Count, max);
        return subList.OrderBy(_ => Guid.NewGuid()).Take(count).ToList();
    }

    public async Task ResolveAndAddKPIDetailsAsync(Appraisal appraisal, long cycleId)
    {
        if (appraisal.EmployeeId.HasValue)
        {
            await ResolveEmployeeKpiDetailsAsync(appraisal, cycleId);
        }
        else if (appraisal.EntityType == AppraisalConstants.EntityTypes.Department && appraisal.EntityId.HasValue)
        {
            await ResolveEntityKpiDetailsAsync(appraisal, AppraisalConstants.EntityTypes.Department, appraisal.EntityId.Value);
        }
        else if (appraisal.EntityType == AppraisalConstants.EntityTypes.Team && appraisal.EntityId.HasValue)
        {
            await ResolveEntityKpiDetailsAsync(appraisal, AppraisalConstants.EntityTypes.Team, appraisal.EntityId.Value);
        }
    }

    private async Task ResolveEmployeeKpiDetailsAsync(Appraisal appraisal, long cycleId)
    {
        var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(appraisal.EmployeeId!.Value);
        if (employment == null) return;

        var employeeKPIs = await _uow.Perf.EmployeeKPIs.GetByEmployeeAndCycleAsync(appraisal.EmployeeId.Value, cycleId);

        var entityKPIs = (await _uow.Perf.EntityKPIs
            .GetByEntityAsync(AppraisalConstants.EntityTypes.Position, employment.PositionId))
            .Where(e => !e.IsDeleted).ToList();

        var usedKPIIds = new HashSet<long>(employeeKPIs.Select(e => e.KPIId));
        var allKPIIds = new HashSet<long>(employeeKPIs.Select(e => e.KPIId));
        foreach (var ek in entityKPIs)
            allKPIIds.Add(ek.KPIId);

        if (allKPIIds.Count == 0) return;

        var kpiMasters = await _uow.Perf.KPIMasters.FindAllAsync(
            k => allKPIIds.Contains(k.Id),
            includes: k => k.Category);

        var kpiDict = kpiMasters.ToDictionary(k => k.Id);

        foreach (var ekpi in employeeKPIs)
        {
            if (!kpiDict.TryGetValue(ekpi.KPIId, out var kpi)) continue;
            var detail = new AppraisalDetail(
                appraisal.Id, ekpi.KPIId, kpi.Name, kpi.Category?.Name,
                ekpi.Weightage, ekpi.TargetValue,
                scoringDirection: kpi.ScoringDirection,
                employeeKPIId: ekpi.Id);
            appraisal.AddDetail(detail);
        }

        foreach (var ekpi in entityKPIs)
        {
            if (usedKPIIds.Contains(ekpi.KPIId)) continue;
            if (!kpiDict.TryGetValue(ekpi.KPIId, out var kpi)) continue;
            var detail = new AppraisalDetail(
                appraisal.Id, ekpi.KPIId, kpi.Name, kpi.Category?.Name,
                ekpi.Weightage, ekpi.TargetValue,
                scoringDirection: kpi.ScoringDirection);
            appraisal.AddDetail(detail);
        }
    }

    private async Task ResolveEntityKpiDetailsAsync(Appraisal appraisal, string entityType, long entityId)
    {
        var entityKPIs = (await _uow.Perf.EntityKPIs
            .GetByEntityAsync(entityType, entityId))
            .Where(e => !e.IsDeleted).ToList();

        if (entityKPIs.Count == 0) return;

        var kpiIds = entityKPIs.Select(e => e.KPIId).ToHashSet();
        var kpiMasters = await _uow.Perf.KPIMasters.FindAllAsync(
            k => kpiIds.Contains(k.Id),
            includes: k => k.Category);
        var kpiDict = kpiMasters.ToDictionary(k => k.Id);

        foreach (var ekpi in entityKPIs)
        {
            if (!kpiDict.TryGetValue(ekpi.KPIId, out var kpi)) continue;
            var detail = new AppraisalDetail(
                appraisal.Id, ekpi.KPIId, kpi.Name, kpi.Category?.Name,
                ekpi.Weightage, ekpi.TargetValue,
                scoringDirection: kpi.ScoringDirection);
            appraisal.AddDetail(detail);
        }
    }
}
