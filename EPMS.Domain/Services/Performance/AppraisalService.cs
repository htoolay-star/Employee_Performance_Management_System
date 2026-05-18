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

        var employee = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (employee == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(dto.EmployeeId), ErrorType.NotFound);

        var managerReviewer = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.ManagerReviewerId);
        if (managerReviewer == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(dto.ManagerReviewerId), ErrorType.NotFound);

        var hasExisting = await _uow.Perf.Appraisals.ExistsByEmployeeAndCycleAsync(dto.EmployeeId, dto.CycleId);
        if (hasExisting)
            return SuccessResponse.Fail(AppraisalMsg.DuplicateEntry, ErrorType.Conflict);

        var appraisal = new Appraisal(dto.EmployeeId, dto.CycleId, dto.ManagerReviewerId);

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
        return SuccessResponse<AppraisalDto>.Ok(dto, AppraisalMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAllAsync()
    {
        var appraisals = await _uow.Perf.Appraisals.GetAllAsync();
        var dtos = appraisals.Where(a => !a.IsDeleted).Adapt<IEnumerable<AppraisalDto>>();
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByEmployeeIdAsync(long employeeId)
    {
        var employee = await _uow.Info.EmployeeProfiles.GetByIdAsync(employeeId);
        if (employee == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(employeeId), ErrorType.NotFound);

        var appraisals = await _uow.Perf.Appraisals.GetEmployeeAppraisalsAsync(employeeId, 0);
        var dtos = appraisals.Where(a => !a.IsDeleted).Adapt<IEnumerable<AppraisalDto>>();
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedByEmployee);
    }

    public async Task<SuccessResponse> GetAppraisalFillAsync(long id)
    {
        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(id), ErrorType.NotFound);

            var dto = new AppraisalFillDto
            {
                Id = appraisal.Id,
                EmployeeId = appraisal.EmployeeId,
                EmployeeName = appraisal.Employee?.StaffName,
                CycleId = appraisal.CycleId,
                CycleName = appraisal.Cycle?.Name,
                ManagerReviewerId = appraisal.ManagerReviewerId,
                ManagerReviewerName = appraisal.ManagerReviewer?.StaffName,
                AppraisalReviewerId = appraisal.Cycle?.AppraisalReviewerId,
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

        if (appraisal.Cycle?.AppraisalReviewerId == null || currentEmployeeId != appraisal.Cycle.AppraisalReviewerId)
            return SuccessResponse.Fail("You are not authorized to submit this appraisal.", ErrorType.Forbidden);

        if (appraisal.Cycle != null)
        {
            var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().DateTime);
            var start = appraisal.Cycle.ManagerReviewStartDate ?? appraisal.Cycle.WindowStartDate;
            var deadline = appraisal.Cycle.ManagerReviewDeadline ?? appraisal.Cycle.WindowEndDate;

            if (today < start)
                return SuccessResponse.Fail($"Manager review window opens on {start:dd/MM/yyyy}.", ErrorType.Validation);
            if (today > deadline)
                return SuccessResponse.Fail($"Manager review window closed on {deadline:dd/MM/yyyy}.", ErrorType.Validation);
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
                if (appraisal.ManagerLockIsDeadline)
                    return SuccessResponse.Fail("Cannot unlock deadline-locked manager evaluation.", ErrorType.Conflict);
                appraisal.UnlockManager();
                break;
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
        var allEmployeeKPIs = await _uow.Perf.EmployeeKPIs
            .FindAllAsync(k => k.CycleId == cycleId && !k.IsDeleted);

        var employeeGroups = allEmployeeKPIs.GroupBy(k => k.EmployeeId);
        var created = 0;
        var skipped = 0;

        foreach (var group in employeeGroups)
        {
            var employeeId = group.Key;

            var exists = await _uow.Perf.Appraisals.ExistsByEmployeeAndCycleAsync(employeeId, cycleId);
            if (exists) { skipped++; continue; }

            var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(employeeId);
            if (employment?.EmploymentStatus != EmploymentStatuses.Permanent
                || !employment.DateOfAppointment.HasValue
                || employment.DateOfAppointment.Value > cycle.EvaluationStartDate
                || (employment.DateOfTermination.HasValue && employment.DateOfTermination.Value <= cycle.EvaluationEndDate))
            {
                skipped++; continue;
            }

            if (employment?.DirectManagerId == null) { skipped++; continue; }

            var managerReviewerId = employment.DirectManagerId.Value;
            var appraisal = new Appraisal(employeeId, cycleId, managerReviewerId);

            await ResolveAndAddKPIDetailsAsync(appraisal, cycleId);

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
                        _uow.Perf.EvaluationResponses.Add(response);
                    }
                }
            }

            _uow.Perf.Appraisals.Add(appraisal);
            await _uow.CompleteAsync();
            created++;
        }

        return (created, skipped);
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

        // All questions in the same form share the same QuestionRatingScale
        var anyResponse = responses.FirstOrDefault(r => r.RatingValue.HasValue);
        var maxScale = 5m;
        if (anyResponse != null)
        {
            var question = await _uow.Perf.FormQuestions.FindAsync(
                q => q.Id == anyResponse.QuestionId,
                includes: q => q.RatingScale);
            maxScale = question?.RatingScale != null && question.RatingScale.Levels.Any()
                ? Math.Max(question.RatingScale.Levels.Max(l => l.Rating), 1m)
                : 5m;
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
                if (cycle?.AppraisalReviewerId.HasValue == true)
                    entries.Add((cycle.AppraisalReviewerId.Value, EvaluatorRoles.Appraisal));
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
        var employment = await _uow.Info.EmployeeEmployments.GetByEmployeeIdAsync(appraisal.EmployeeId);
        if (employment == null) return;

        var employeeKPIs = await _uow.Perf.EmployeeKPIs.GetByEmployeeAndCycleAsync(appraisal.EmployeeId, cycleId);

        var entityKPIs = new List<EntityKPI>();
        entityKPIs.AddRange(await _uow.Perf.EntityKPIs.GetByEntityAsync(AppraisalConstants.EntityTypes.Position, employment.PositionId));
        entityKPIs.AddRange(await _uow.Perf.EntityKPIs.GetByEntityAsync(AppraisalConstants.EntityTypes.Department, employment.DepartmentId));
        if (employment.TeamId.HasValue)
            entityKPIs.AddRange(await _uow.Perf.EntityKPIs.GetByEntityAsync(AppraisalConstants.EntityTypes.Team, employment.TeamId.Value));

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
}
