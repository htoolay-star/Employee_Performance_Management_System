using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Enums;
using Mapster;
using System.Linq.Expressions;
using static EPMS.Shared.Constants.ServiceResponseMessages;
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

        var myAppraisals = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.Employee != null && a.Employee.Employment != null
                 && a.Employee.Employment.DirectManagerId == currentEmployeeId.Value
                 && a.EmployeeId != null && !a.IsDeleted,
            includes: new Expression<Func<Appraisal, object>>[]
            {
                a => a.Employee,
                a => a.Employee.Employment,
                a => a.Cycle,
                a => a.ManagerReviewer
            }
        );

        var allAppraisals = myAppraisals.ToList();

        if (await IsCurrentUserAdminAsync())
        {
            var noManagerAppraisals = await _uow.Perf.Appraisals.FindAllAsync(
                a => a.Employee != null && a.Employee.Employment != null
                     && a.Employee.Employment.DirectManagerId == null && !a.IsDeleted,
                includes: new Expression<Func<Appraisal, object>>[]
                {
                    a => a.Employee,
                    a => a.Cycle,
                    a => a.ManagerReviewer
                }
            );

            foreach (var a in noManagerAppraisals)
            {
                if (!allAppraisals.Any(x => x.Id == a.Id))
                    allAppraisals.Add(a);
            }
        }

        var dtos = allAppraisals.Select(MapToDto).ToList();
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByEntityTypeAndCycleAsync(string entityType, long cycleId)
    {
        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(cycleId);
        if (cycle == null)
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(cycleId), ErrorType.NotFound);

        var appraisals = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.EntityType == entityType && a.CycleId == cycleId && !a.IsDeleted,
            false, default, a => a.Details, a => a.Cycle);

        var entities = appraisals.ToList();
        var dtos = entities.Select(MapToDto).ToList();

        var entityIds = entities
            .Where(a => a.EntityId.HasValue && a.EntityType == entityType)
            .Select(a => a.EntityId!.Value)
            .Distinct()
            .ToList();

        if (entityIds.Count != 0)
        {
            if (entityType == AppraisalConstants.EntityTypes.Department)
            {
                var depts = await _uow.HR.Departments.FindAllAsync(
                    d => entityIds.Contains(d.Id),
                    includes: d => d.DeptHead);
                var deptDict = depts.ToDictionary(d => d.Id);
                for (var i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto.EntityId.HasValue && deptDict.TryGetValue(dto.EntityId.Value, out var dept))
                        dtos[i] = dto with { EntityName = dept.Name, EntityHeadName = dept.DeptHead?.StaffName };
                }
            }
            else if (entityType == AppraisalConstants.EntityTypes.Team)
            {
                var teams = await _uow.HR.Teams.FindAllAsync(
                    t => entityIds.Contains(t.Id),
                    includes: t => t.LeadTeam);
                var teamDict = teams.ToDictionary(t => t.Id);
                for (var i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto.EntityId.HasValue && teamDict.TryGetValue(dto.EntityId.Value, out var team))
                        dtos[i] = dto with { EntityName = team.Name, EntityHeadName = team.LeadTeam?.StaffName };
                }
            }
        }

        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByEntityTypeAsync(string entityType)
    {
        var appraisals = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.EntityType == entityType && !a.IsDeleted,
            false, default, a => a.Details, a => a.Cycle);

        var entities = appraisals.ToList();
        var dtos = entities.Select(MapToDto).ToList();

        var entityIds = entities
            .Where(a => a.EntityId.HasValue && a.EntityType == entityType)
            .Select(a => a.EntityId!.Value)
            .Distinct()
            .ToList();

        if (entityIds.Count != 0)
        {
            if (entityType == AppraisalConstants.EntityTypes.Department)
            {
                var depts = await _uow.HR.Departments.FindAllAsync(
                    d => entityIds.Contains(d.Id),
                    includes: d => d.DeptHead);
                var deptDict = depts.ToDictionary(d => d.Id);
                for (var i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto.EntityId.HasValue && deptDict.TryGetValue(dto.EntityId.Value, out var dept))
                        dtos[i] = dto with { EntityName = dept.Name, EntityHeadName = dept.DeptHead?.StaffName };
                }
            }
            else if (entityType == AppraisalConstants.EntityTypes.Team)
            {
                var teams = await _uow.HR.Teams.FindAllAsync(
                    t => entityIds.Contains(t.Id),
                    includes: t => t.LeadTeam);
                var teamDict = teams.ToDictionary(t => t.Id);
                for (var i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto.EntityId.HasValue && teamDict.TryGetValue(dto.EntityId.Value, out var team))
                        dtos[i] = dto with { EntityName = team.Name, EntityHeadName = team.LeadTeam?.StaffName };
                }
            }
        }

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
            EntityHeadName: null,
            CycleId: appraisal.CycleId,
            CycleName: appraisal.Cycle?.Name,
            ManagerReviewerId: appraisal.ManagerReviewerId,
            ManagerReviewerName: appraisal.ManagerReviewer?.StaffName,
            Status: appraisal.Status ?? "Draft",
            KpiStatus: appraisal.KpiStatus ?? AppraisalStatuses.Kpi.Draft,
            SelfStatus: appraisal.SelfStatus ?? AppraisalStatuses.Self.Draft,
            ManagerStatus: appraisal.ManagerStatus ?? AppraisalStatuses.Manager.Draft,
            PeerStatus: appraisal.PeerStatus ?? AppraisalStatuses.Peer.Draft,
            SubordinateStatus: appraisal.SubordinateStatus ?? AppraisalStatuses.Subordinate.Draft,
            CommitteeStatus: appraisal.CommitteeStatus ?? AppraisalStatuses.Committee.Draft,
            RatingLabel: appraisal.RatingLabel,
            TotalScore: appraisal.TotalScore,
            KpiScore: appraisal.KpiScore,
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

        string? entityName = null;
        string? entityHeadName = null;

        switch (dto.EntityType)
        {
            case AppraisalConstants.EntityTypes.Department:
                var dept = await _uow.HR.Departments.FindAsync(
                    d => d.Id == dto.EntityId.Value,
                    includes: d => d.DeptHead);
                entityName = dept?.Name;
                entityHeadName = dept?.DeptHead?.StaffName;
                break;
            case AppraisalConstants.EntityTypes.Team:
                var team = await _uow.HR.Teams.FindAsync(
                    t => t.Id == dto.EntityId.Value,
                    includes: t => t.LeadTeam);
                entityName = team?.Name;
                entityHeadName = team?.LeadTeam?.StaffName;
                break;
        }

        return dto with { EntityName = entityName, EntityHeadName = entityHeadName };
    }

    public async Task<SuccessResponse> GetAppraisalFillAsync(long id)
    {
        var dto = await _uow.Perf.Appraisals.GetAppraisalFillDtoAsync(id);
        if (dto == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue)
            return SuccessResponse.Fail("User identity not found.", ErrorType.Forbidden);

        var isDirectManager = currentEmployeeId.Value == dto.DirectManagerId;
        var hasNoManager = dto.DirectManagerId == null;

        bool isAuthorized = isDirectManager || (hasNoManager && await IsCurrentUserAdminAsync());

        if (!isAuthorized)
            return SuccessResponse.Fail("Only the direct manager can evaluate KPI.", ErrorType.Forbidden);

        return SuccessResponse<AppraisalFillDto>.Ok(dto, AppraisalMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAppraisalViewAsync(long id)
    {
        var dto = await _uow.Perf.Appraisals.GetAppraisalFillDtoAsync(id);
        if (dto == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        return SuccessResponse<AppraisalFillDto>.Ok(dto, AppraisalMsg.Retrieved);
    }

    public async Task<SuccessResponse> SubmitAsync(AppraisalSubmissionDto dto)
    {
        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue)
            return SuccessResponse.Fail("User identity not found.", ErrorType.Forbidden);

        var validationAppraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(dto.Id);
        if (validationAppraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(dto.Id), ErrorType.NotFound);

        if (validationAppraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        var hasNoManager = validationAppraisal.Employee?.Employment?.DirectManagerId == null;
        var isAdmin = await IsCurrentUserAdminAsync();
        var directManagerId = validationAppraisal.Employee?.Employment?.DirectManagerId;

        if (currentEmployeeId != directManagerId)
        {
            if (!(hasNoManager && isAdmin))
                return SuccessResponse.Fail("You are not authorized to submit this appraisal.", ErrorType.Forbidden);
        }

        if (validationAppraisal.Cycle != null)
        {
            var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
            var start = validationAppraisal.Cycle.KpiReviewStartDate ?? validationAppraisal.Cycle.WindowStartDate;
            var deadline = validationAppraisal.Cycle.KpiReviewDeadline ?? validationAppraisal.Cycle.WindowEndDate;

            if (today < start)
                return SuccessResponse.Fail($"Appraisal review window opens on {start:dd/MM/yyyy}.", ErrorType.Validation);
            if (today > deadline)
                return SuccessResponse.Fail($"Appraisal review window closed on {deadline:dd/MM/yyyy}.", ErrorType.Validation);
        }

        var trackedAppraisal = (await _uow.Perf.Appraisals.FindAllAsync(
            a => a.Id == dto.Id,
            trackChanges: true,
            includes: a => a.Details)).FirstOrDefault();

        if (trackedAppraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(dto.Id), ErrorType.NotFound);

        foreach (var detailDto in dto.Details)
        {
            var detail = trackedAppraisal.Details.FirstOrDefault(d =>
                d.KPIId == detailDto.KPIId);

            if (detail != null)
            {
                detail.Evaluate(detailDto.ActualValue, detailDto.Comment);
            }
        }

        bool allDone = false;

        if (trackedAppraisal.KpiStatus is AppraisalStatuses.Kpi.Draft)
        {
            trackedAppraisal.LockKpi(isDeadline: false);
            if (hasNoManager && isAdmin)
            {
                trackedAppraisal.SetKpiStatus(AppraisalStatuses.Kpi.Finalized);
                if (trackedAppraisal.UpdateOverallStatusIfAllDone(_timeProvider))
                    allDone = true;
            }
            else
            {
                trackedAppraisal.UpdateDetails(status: AppraisalStatuses.Kpi.Reviewed,
                    employeeComment: null, managerComment: null, ratingLabel: null);
            }
        }

        await _uow.CompleteAsync();

        if (allDone)
            await CalculateAndStoreFinalScoreAsync(dto.Id);

        return SuccessResponse.Ok(hasNoManager && isAdmin ? AppraisalMsg.Locked : AppraisalMsg.Submitted);
    }

    public async Task<SuccessResponse> GetMyKpiAsync()
    {
        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue)
            return SuccessResponse.Fail("User identity not found.", ErrorType.Forbidden);

        var appraisals = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.EmployeeId == currentEmployeeId.Value && !a.IsDeleted,
            includes: new Expression<Func<Appraisal, object>>[]
            {
                a => a.Employee,
                a => a.Employee.Employment,
                a => a.Employee.Employment.DirectManager,
                a => a.Cycle,
                a => a.Details
            }
        );

        var dtos = appraisals.Select(a => new AppraisalFillDto
        {
            Id = a.Id,
            EmployeeId = a.EmployeeId ?? 0,
            EmployeeName = a.Employee?.StaffName,
            StaffNo = a.Employee?.StaffNo ?? string.Empty,
            PositionName = a.Employee?.Employment?.Position?.Name,
            DepartmentName = a.Employee?.Employment?.Department?.Name,
            TeamName = a.Employee?.Employment?.Team?.Name,
            ManagerName = a.Employee?.Employment?.DirectManager?.StaffName ?? "Admin Team",
            CycleId = a.CycleId,
            CycleName = a.Cycle?.Name,
            ManagerReviewerId = a.ManagerReviewerId,
            ManagerReviewerName = a.ManagerReviewer?.StaffName,
            Status = a.Status,
            IsLocked = a.IsLocked,
            KpiLocked = a.KpiLocked,
            KpiStatus = a.KpiStatus ?? AppraisalStatuses.Kpi.Draft,
            SelfStatus = a.SelfStatus ?? AppraisalStatuses.Self.Draft,
            ManagerStatus = a.ManagerStatus ?? AppraisalStatuses.Manager.Draft,
            PeerStatus = a.PeerStatus ?? AppraisalStatuses.Peer.Draft,
            SubordinateStatus = a.SubordinateStatus ?? AppraisalStatuses.Subordinate.Draft,
            CommitteeStatus = a.CommitteeStatus ?? AppraisalStatuses.Committee.Draft,
            Details = a.Details.Select(d => new AppraisalDetailFillDto
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
        }).ToList();

        return SuccessResponse<IEnumerable<AppraisalFillDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetPendingAsync()
    {
        var appraisals = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.KpiStatus == AppraisalStatuses.Kpi.Reviewed && !a.IsDeleted,
            includes: new Expression<Func<Appraisal, object>>[]
            {
                a => a.Employee,
                a => a.Cycle
            }
        );

        var dtos = appraisals.Select(MapToDto).ToList();
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
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
            case "KPI":
                if (appraisal.KpiLockIsDeadline)
                    return SuccessResponse.Fail("Cannot unlock deadline-locked KPI.", ErrorType.Conflict);
                appraisal.UnlockKpi();
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

        // 3. Exclude SystemAdmin employees
        var saEmployeeIds = (await _uow.Auth.Users
            .FindAllAsync(u => u.RoleId == (long)UserRole.SystemAdmin && !u.IsDeleted,
                          includes: u => u.Profile))
            .Where(u => u.Profile != null)
            .Select(u => u.Profile!.Id)
            .ToHashSet();

        // 4. Process each employee
        foreach (var employeeId in employeeIds)
        {
            if (saEmployeeIds.Contains(employeeId)) { skipped++; continue; }

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

            long managerReviewerId;
            if (employment.DirectManagerId == null)
            {
                var adminId = await GetDefaultReviewerIdAsync();
                if (adminId == null) { skipped++; continue; }
                managerReviewerId = adminId.Value;
            }
            else
            {
                managerReviewerId = employment.DirectManagerId.Value;
            }
            var appraisal = new Appraisal(employeeId, cycleId, managerReviewerId);

            await ResolveAndAddKPIDetailsAsync(appraisal, cycleId);
            _uow.Perf.Appraisals.Add(appraisal);

            var positionTemplates = await _uow.Perf.PositionFormTemplates
                .GetByPositionIdWithQuestionsAsync(employment.PositionId);

            foreach (var pt in positionTemplates)
            {
                var template = pt.FormTemplate;
                if (template?.Questions == null || !template.IsActive) continue;

                var entries = await ResolveEvaluatorEntriesAsync(employment, template.FormType, employeeId, cycle);

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

        // 5. Generate entity appraisals for Departments with EntityKPIs
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

            var managerId = await GetDefaultReviewerIdAsync();
            if (managerId == null) { skipped++; continue; }

            var appraisal = new Appraisal(
                AppraisalConstants.EntityTypes.Department, deptId, cycleId, managerId.Value);

            await ResolveAndAddKPIDetailsAsync(appraisal, cycleId);

            _uow.Perf.Appraisals.Add(appraisal);
            await _uow.CompleteAsync();
            created++;
        }

        // 6. Generate entity appraisals for Teams with EntityKPIs
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

            var managerId = await GetDefaultReviewerIdAsync();
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
        var setting = await _uow.App.SystemSettings.GetByKeyAsync(SettingKeys.AdminPositionId);
        if (setting != null && long.TryParse(setting.Value, out var adminPositionId))
        {
            var adminUser = await _uow.Auth.Users.FindAsync(
                u => u.PositionId == adminPositionId && !u.IsDeleted,
                includes: u => u.Profile);
            if (adminUser?.Profile != null)
                return adminUser.Profile.Id;
        }

        var systemAdmin = await _uow.Auth.Users.FindAsync(
            u => u.RoleId == (long)UserRole.SystemAdmin && !u.IsDeleted,
            includes: u => u.Profile);
        return systemAdmin?.Profile?.Id;
    }

    public async Task<SuccessResponse> FinalizeKpiAsync(long id)
    {
        if (!await IsCurrentUserAdminAsync())
            return SuccessResponse.Fail("Only administrators can finalize appraisals.", ErrorType.Forbidden);

        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        appraisal.LockKpi(isDeadline: false);
        appraisal.SetKpiStatus(AppraisalStatuses.Kpi.Finalized);

        await _uow.CompleteAsync();

        if (appraisal.UpdateOverallStatusIfAllDone(_timeProvider))
            await CalculateAndStoreFinalScoreAsync(id);

        return SuccessResponse.Ok(AppraisalMsg.Locked);
    }

    public async Task<SuccessResponse> FinalizeEvaluationAsync(long appraisalId, string role)
    {
        if (!await IsCurrentUserAdminAsync())
            return SuccessResponse.Fail("Only administrators can finalize evaluations.", ErrorType.Forbidden);

        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(appraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        switch (role)
        {
            case EvaluatorRoles.Self:
                appraisal.SetSelfStatus(AppraisalStatuses.Self.Finalized);
                break;
            case EvaluatorRoles.Manager:
                appraisal.SetManagerStatus(AppraisalStatuses.Manager.Finalized);
                break;
            case EvaluatorRoles.Peer:
                appraisal.SetPeerStatus(AppraisalStatuses.Peer.Finalized);
                break;
            case EvaluatorRoles.Subordinate:
                appraisal.SetSubordinateStatus(AppraisalStatuses.Subordinate.Finalized);
                break;
            case EvaluatorRoles.Appraisal:
                appraisal.SetCommitteeStatus(AppraisalStatuses.Committee.Finalized);
                break;
            default:
                return SuccessResponse.Fail($"Unknown role: {role}", ErrorType.Validation);
        }

        bool allDone = appraisal.UpdateOverallStatusIfAllDone(_timeProvider);

        await _uow.CompleteAsync();

        if (allDone)
            await CalculateAndStoreFinalScoreAsync(appraisalId);

        return SuccessResponse.Ok(AppraisalMsg.Finalized);
    }

    public async Task AutoFinalizeAndCalculateScoreAsync(long appraisalId)
    {
        var appraisal = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.Id == appraisalId, trackChanges: true,
            includes: new Expression<Func<Appraisal, object>>[] { a => a.Details, a => a.Cycle });
        var tracked = appraisal.FirstOrDefault();
        if (tracked == null) return;

        if (!tracked.UpdateOverallStatusIfAllDone(_timeProvider))
            return;

        await CalculateAndStoreFinalScoreAsync(tracked);
        await _uow.CompleteAsync();
    }

    private async Task CalculateAndStoreFinalScoreAsync(long appraisalId)
    {
        var appraisal = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.Id == appraisalId, trackChanges: true,
            includes: new Expression<Func<Appraisal, object>>[] { a => a.Details, a => a.Cycle });
        var tracked = appraisal.FirstOrDefault();
        if (tracked == null) return;
        await CalculateAndStoreFinalScoreAsync(tracked);
    }

    private async Task CalculateAndStoreFinalScoreAsync(Appraisal appraisal)
    {
        var kpiScore = appraisal.Details
            .Where(d => d.KPIId.HasValue && d.Score > 0)
            .Select(d => d.WeightedScore)
            .DefaultIfEmpty(0)
            .Average();

        var responses = await _uow.Perf.EvaluationResponses
            .FindAllAsync(r => r.AppraisalId == appraisal.Id && !r.IsDeleted,
                includes: r => r.Question);

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

        if (appraisal.Cycle == null) return;

        var ratingScales = await _uow.Perf.RatingScales
            .FindAllAsync(s => s.IsActive && !s.IsDeleted);

        var totalBeforeMatch = (kpiScore * appraisal.Cycle.KpiWeight / 100m)
                             + (selfScore * appraisal.Cycle.SelfWeight / 100m)
                             + (threeSixtyScore * appraisal.Cycle.ThreeSixtyWeight / 100m)
                             + (appraisalScore * appraisal.Cycle.AppraisalWeight / 100m);

        var matchingScale = ratingScales.FirstOrDefault(s => s.IsMatch(totalBeforeMatch));
        if (matchingScale == null) return;

        appraisal.SetComputedScores(
            kpiScore, selfScore, threeSixtyScore, appraisalScore,
            appraisal.Cycle.KpiWeight, appraisal.Cycle.SelfWeight,
            appraisal.Cycle.ThreeSixtyWeight, appraisal.Cycle.AppraisalWeight,
            matchingScale);
    }

    public async Task<SuccessResponse> GetManagerSelfPendingAsync()
    {
        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue)
            return SuccessResponse.Fail("User identity not found.", ErrorType.Forbidden);

        var appraisals = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.Employee != null && a.Employee.Employment != null
                 && a.Employee.Employment.DirectManagerId == currentEmployeeId.Value
                 && a.SelfStatus == AppraisalStatuses.Self.InProgress
                 && !a.IsDeleted,
            includes: new Expression<Func<Appraisal, object>>[] { a => a.Employee, a => a.Employee.Employment, a => a.Cycle });

        var allAppraisals = appraisals.ToList();

        if (await IsCurrentUserAdminAsync())
        {
            var noManagerPending = await _uow.Perf.Appraisals.FindAllAsync(
                a => a.Employee != null && a.Employee.Employment != null
                     && a.Employee.Employment.DirectManagerId == null
                     && a.SelfStatus == AppraisalStatuses.Self.InProgress
                     && !a.IsDeleted,
                includes: new Expression<Func<Appraisal, object>>[] { a => a.Employee, a => a.Employee.Employment, a => a.Cycle });

            foreach (var a in noManagerPending)
            {
                if (!allAppraisals.Any(x => x.Id == a.Id))
                    allAppraisals.Add(a);
            }

            var reviewed = await _uow.Perf.Appraisals.FindAllAsync(
                a => a.Employee != null && a.Employee.Employment != null
                     && a.SelfStatus == AppraisalStatuses.Self.Reviewed
                     && !a.IsDeleted,
                includes: new Expression<Func<Appraisal, object>>[] { a => a.Employee, a => a.Employee.Employment, a => a.Cycle });

            foreach (var a in reviewed)
            {
                if (!allAppraisals.Any(x => x.Id == a.Id))
                    allAppraisals.Add(a);
            }
        }

        var dtos = allAppraisals.Select(MapToDto).ToList();
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, "Pending self assessments retrieved.");
    }

    public async Task<SuccessResponse> ApproveSelfAssessmentAsync(long appraisalId)
    {
        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue)
            return SuccessResponse.Fail("User identity not found.", ErrorType.Forbidden);

        var appraisal = await _uow.Perf.Appraisals.FindAllAsync(
            a => a.Id == appraisalId, trackChanges: true,
            includes: new Expression<Func<Appraisal, object>>[] { a => a.Employee, a => a.Employee.Employment, a => a.Cycle });
        var tracked = appraisal.FirstOrDefault();
        if (tracked == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        if (tracked.Employee?.Employment?.DirectManagerId != currentEmployeeId.Value)
        {
            var hasNoManager = tracked.Employee?.Employment?.DirectManagerId == null;
            if (!(hasNoManager && await IsCurrentUserAdminAsync()))
                return SuccessResponse.Fail("Only the direct manager can approve the self assessment.", ErrorType.Forbidden);
        }

        if (tracked.SelfStatus != AppraisalStatuses.Self.InProgress)
            return SuccessResponse.Fail("Self assessment must be InProgress to approve.", ErrorType.Validation);

        tracked.ApproveSelf();

        bool allDone = tracked.UpdateOverallStatusIfAllDone(_timeProvider);

        await _uow.CompleteAsync();

        if (allDone)
            await CalculateAndStoreFinalScoreAsync(appraisalId);

        return SuccessResponse.Ok("Self assessment approved successfully.");
    }

    public async Task<SuccessResponse> GetEmployeeFormsOverviewAsync(long appraisalId)
    {
        var appraisal = await _uow.Perf.Appraisals.FindAsync(
            a => a.Id == appraisalId && !a.IsDeleted,
            false, default,
            a => a.Cycle,
            a => a.Employee.Employment.Position,
            a => a.Employee.Employment.Department,
            a => a.Employee.Employment.Team,
            a => a.Employee.Employment.DirectManager,
            a => a.ManagerReviewer);

        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        var dto = new EmployeeFormsOverviewDto
        {
            AppraisalId = appraisal.Id,
            EmployeeId = appraisal.EmployeeId,
            EmployeeName = appraisal.Employee?.StaffName,
            CycleName = appraisal.Cycle?.Name,
            PositionName = appraisal.Employee?.Employment?.Position?.Name,
            DepartmentName = appraisal.Employee?.Employment?.Department?.Name,
            TeamName = appraisal.Employee?.Employment?.Team?.Name,
            ManagerName = appraisal.ManagerReviewer?.StaffName,
        };

        var hasManager = appraisal.Employee?.Employment?.DirectManager != null;

        dto.Forms.Add(new FormEntryDto
        {
            FormType = "KPI",
            DisplayName = "KPI",
            Status = appraisal.KpiStatus ?? AppraisalStatuses.Kpi.Draft,
            IsSubmitted = appraisal.KpiStatus != null && appraisal.KpiStatus != AppraisalStatuses.Kpi.Draft,
            IsLocked = appraisal.KpiLocked,
            CanFill = !hasManager && appraisal.KpiStatus == AppraisalStatuses.Kpi.Draft && !appraisal.KpiLocked,
            Score = appraisal.KpiScore,
        });

        dto.Forms.Add(new FormEntryDto
        {
            FormType = EvaluatorRoles.Self,
            DisplayName = "Self Assessment",
            Status = appraisal.SelfStatus ?? AppraisalStatuses.Self.Draft,
            IsSubmitted = appraisal.SelfStatus != null && appraisal.SelfStatus != AppraisalStatuses.Self.Draft,
            IsLocked = appraisal.SelfLocked,
            Score = appraisal.SelfScore,
        });

        dto.Forms.Add(new FormEntryDto
        {
            FormType = EvaluatorRoles.Manager,
            DisplayName = "Manager Review",
            Status = appraisal.ManagerStatus ?? AppraisalStatuses.Manager.Draft,
            IsSubmitted = appraisal.ManagerStatus != null && appraisal.ManagerStatus != AppraisalStatuses.Manager.Draft,
            IsLocked = appraisal.ThreeSixtyLocked,
        });

        var peerResponses = (await _uow.Perf.EvaluationResponses
            .FindAllAsync(r => r.AppraisalId == appraisalId && r.EvaluatorRole == EvaluatorRoles.Peer && !r.IsDeleted,
                          trackChanges: false,
                          includes: r => r.Evaluator))
            .GroupBy(r => r.EvaluatorId)
            .ToList();

        if (peerResponses.Count != 0)
        {
            foreach (var group in peerResponses)
            {
                var evaluator = group.First().Evaluator;
                var anySubmitted = group.Any(r => r.SubmittedAt != null);
                dto.Forms.Add(new FormEntryDto
                {
                    FormType = EvaluatorRoles.Peer,
                    DisplayName = $"Peer Review - {evaluator?.StaffName ?? $"Employee #{group.Key}"}",
                    Status = anySubmitted ? AppraisalStatuses.Peer.Finalized : AppraisalStatuses.Peer.Draft,
                    IsSubmitted = anySubmitted,
                    IsLocked = appraisal.ThreeSixtyLocked,
                    EvaluatorId = group.Key,
                    EvaluatorName = evaluator?.StaffName,
                });
            }
        }
        else
        {
            dto.Forms.Add(new FormEntryDto
            {
                FormType = EvaluatorRoles.Peer,
                DisplayName = "Peer Review",
                Status = appraisal.PeerStatus ?? AppraisalStatuses.Peer.Draft,
                IsSubmitted = appraisal.PeerStatus != null && appraisal.PeerStatus != AppraisalStatuses.Peer.Draft,
                IsLocked = appraisal.ThreeSixtyLocked,
            });
        }

        dto.Forms.Add(new FormEntryDto
        {
            FormType = EvaluatorRoles.Subordinate,
            DisplayName = "Subordinate Review",
            Status = appraisal.SubordinateStatus ?? AppraisalStatuses.Subordinate.Draft,
            IsSubmitted = appraisal.SubordinateStatus != null && appraisal.SubordinateStatus != AppraisalStatuses.Subordinate.Draft,
            IsLocked = appraisal.ThreeSixtyLocked,
        });

        dto.Forms.Add(new FormEntryDto
        {
            FormType = EvaluatorRoles.Appraisal,
            DisplayName = "Appraisal Review",
            Status = appraisal.CommitteeStatus ?? AppraisalStatuses.Committee.Draft,
            IsSubmitted = appraisal.CommitteeStatus != null && appraisal.CommitteeStatus != AppraisalStatuses.Committee.Draft,
            IsLocked = appraisal.AppraisalLocked,
            CanFill = !hasManager && appraisal.CommitteeStatus == AppraisalStatuses.Committee.Draft && !appraisal.AppraisalLocked,
            Score = appraisal.AppraisalScore,
        });

        return SuccessResponse<EmployeeFormsOverviewDto>.Ok(dto, AppraisalMsg.Retrieved);
    }

    private async Task<List<(long EvaluatorId, string Role)>> ResolveEvaluatorEntriesAsync(
        Entities.EmployeeInfo.EmployeeEmployment employment, string formType,
        long employeeId, AppraisalCycle? cycle)
    {
        var entries = new List<(long, string)>();

        switch (formType)
        {
            case AppraisalConstants.FormTypes.Self:
                entries.Add((employeeId, EvaluatorRoles.Self));
                break;

            case AppraisalConstants.FormTypes.Manager:
                if (employment.DirectManagerId.HasValue)
                    entries.Add((employment.DirectManagerId.Value, EvaluatorRoles.Manager));
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
                if (employment.DirectManagerId.HasValue)
                    entries.Add((employment.DirectManagerId.Value, EvaluatorRoles.Appraisal));
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

    public async Task<SuccessResponse> GetMy360FeedbackAsync(long appraisalId)
    {
        var currentEmployeeId = await _currentEmployee.GetEmployeeIdAsync();
        if (!currentEmployeeId.HasValue)
            return SuccessResponse.Fail("User identity not found.", ErrorType.Forbidden);

        var isAdmin = _currentEmployee.IsAdmin;

        var appraisal = await _uow.Perf.Appraisals.FindAsync(
            a => a.Id == appraisalId && !a.IsDeleted,
            false, default,
            a => a.Cycle,
            a => a.Employee.Employment.Position,
            a => a.Employee.Employment.Department,
            a => a.Employee.Employment.Team,
            a => a.Employee.Employment.DirectManager,
            a => a.ManagerReviewer);

        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        var isEvaluee = appraisal.EmployeeId == currentEmployeeId.Value;
        var isManager = appraisal.Employee?.Employment?.DirectManagerId == currentEmployeeId.Value;

        if (!isAdmin && !isEvaluee && !isManager)
            return SuccessResponse.Fail("Access denied.", ErrorType.Forbidden);

        var dto = new EmployeeFormsOverviewDto
        {
            AppraisalId = appraisal.Id,
            EmployeeId = appraisal.EmployeeId,
            EmployeeName = appraisal.Employee?.StaffName,
            CycleName = appraisal.Cycle?.Name,
            PositionName = appraisal.Employee?.Employment?.Position?.Name,
            DepartmentName = appraisal.Employee?.Employment?.Department?.Name,
            TeamName = appraisal.Employee?.Employment?.Team?.Name,
            ManagerName = appraisal.ManagerReviewer?.StaffName,
        };

        if (isAdmin)
        {
            dto.Forms.Add(new FormEntryDto
            {
                FormType = EvaluatorRoles.Manager,
                DisplayName = "Manager Review",
                Status = appraisal.ManagerStatus ?? AppraisalStatuses.Manager.Draft,
                IsSubmitted = appraisal.ManagerStatus != null && appraisal.ManagerStatus != AppraisalStatuses.Manager.Draft,
                IsLocked = appraisal.ThreeSixtyLocked,
                CanFill = false,
                EvaluatorId = appraisal.Employee?.Employment?.DirectManagerId,
            });

            var peerResponses = (await _uow.Perf.EvaluationResponses
                .FindAllAsync(r => r.AppraisalId == appraisalId && r.EvaluatorRole == EvaluatorRoles.Peer && !r.IsDeleted,
                              trackChanges: false,
                              includes: r => r.Evaluator))
                .GroupBy(r => r.EvaluatorId)
                .ToList();

            if (peerResponses.Count != 0)
            {
                foreach (var group in peerResponses)
                {
                    var evaluator = group.First().Evaluator;
                    var anySubmitted = group.Any(r => r.SubmittedAt != null);
                    dto.Forms.Add(new FormEntryDto
                    {
                        FormType = EvaluatorRoles.Peer,
                        DisplayName = $"Peer Review - {evaluator?.StaffName ?? $"Employee #{group.Key}"}",
                        Status = anySubmitted ? AppraisalStatuses.Peer.Finalized : AppraisalStatuses.Peer.Draft,
                        IsSubmitted = anySubmitted,
                        IsLocked = appraisal.ThreeSixtyLocked,
                        CanFill = false,
                        EvaluatorId = group.Key,
                        EvaluatorName = evaluator?.StaffName,
                    });
                }
            }

            var subordinateGroups = (await _uow.Perf.EvaluationResponses
                .FindAllAsync(r => r.AppraisalId == appraisalId && r.EvaluatorRole == EvaluatorRoles.Subordinate && !r.IsDeleted,
                              trackChanges: false,
                              includes: r => r.Evaluator))
                .GroupBy(r => r.EvaluatorId)
                .ToList();

            if (subordinateGroups.Count != 0)
            {
                foreach (var group in subordinateGroups)
                {
                    var evaluator = group.First().Evaluator;
                    var anySubmitted = group.Any(r => r.SubmittedAt != null);
                    dto.Forms.Add(new FormEntryDto
                    {
                        FormType = EvaluatorRoles.Subordinate,
                        DisplayName = $"Subordinate Review - {evaluator?.StaffName ?? $"Employee #{group.Key}"}",
                        Status = anySubmitted ? AppraisalStatuses.Subordinate.Finalized : AppraisalStatuses.Subordinate.Draft,
                        IsSubmitted = anySubmitted,
                        IsLocked = appraisal.ThreeSixtyLocked,
                        CanFill = false,
                        EvaluatorId = group.Key,
                        EvaluatorName = evaluator?.StaffName,
                    });
                }
            }
        }
        else if (isEvaluee)
        {
            dto.Forms.Add(new FormEntryDto
            {
                FormType = EvaluatorRoles.Manager,
                DisplayName = "Manager Review",
                Status = appraisal.ManagerStatus ?? AppraisalStatuses.Manager.Draft,
                IsSubmitted = appraisal.ManagerStatus != null && appraisal.ManagerStatus != AppraisalStatuses.Manager.Draft,
                IsLocked = appraisal.ThreeSixtyLocked,
                CanFill = false,
                EvaluatorId = appraisal.Employee?.Employment?.DirectManagerId,
                EvaluatorName = "Anonymous",
            });

            var peerResponses = (await _uow.Perf.EvaluationResponses
                .FindAllAsync(r => r.AppraisalId == appraisalId && r.EvaluatorRole == EvaluatorRoles.Peer && !r.IsDeleted,
                              trackChanges: false,
                              includes: r => r.Evaluator))
                .GroupBy(r => r.EvaluatorId)
                .ToList();

            if (peerResponses.Count != 0)
            {
                foreach (var group in peerResponses)
                {
                    var anySubmitted = group.Any(r => r.SubmittedAt != null);
                    dto.Forms.Add(new FormEntryDto
                    {
                        FormType = EvaluatorRoles.Peer,
                        DisplayName = "Peer Review",
                        Status = anySubmitted ? AppraisalStatuses.Peer.Finalized : AppraisalStatuses.Peer.Draft,
                        IsSubmitted = anySubmitted,
                        IsLocked = appraisal.ThreeSixtyLocked,
                        CanFill = false,
                        EvaluatorId = group.Key,
                        EvaluatorName = "Anonymous",
                    });
                }
            }
        }
        else if (isManager)
        {
            var subordinateGroups = (await _uow.Perf.EvaluationResponses
                .FindAllAsync(r => r.AppraisalId == appraisalId && r.EvaluatorRole == EvaluatorRoles.Subordinate && !r.IsDeleted,
                              trackChanges: false,
                              includes: r => r.Evaluator))
                .GroupBy(r => r.EvaluatorId)
                .ToList();

            if (subordinateGroups.Count != 0)
            {
                foreach (var group in subordinateGroups)
                {
                    var anySubmitted = group.Any(r => r.SubmittedAt != null);
                    dto.Forms.Add(new FormEntryDto
                    {
                        FormType = EvaluatorRoles.Subordinate,
                        DisplayName = "Subordinate Review",
                        Status = anySubmitted ? AppraisalStatuses.Subordinate.Finalized : AppraisalStatuses.Subordinate.Draft,
                        IsSubmitted = anySubmitted,
                        IsLocked = appraisal.ThreeSixtyLocked,
                        CanFill = false,
                        EvaluatorId = group.Key,
                        EvaluatorName = "Anonymous",
                    });
                }
            }
        }

        if (dto.Forms.Count == 0)
            return SuccessResponse.Fail("No 360 feedback available.", ErrorType.NotFound);

        return SuccessResponse<EmployeeFormsOverviewDto>.Ok(dto, "360 feedback retrieved.");
    }

    private Task<bool> IsCurrentUserAdminAsync()
    {
        return Task.FromResult(_currentEmployee.IsAdmin);
    }
}
