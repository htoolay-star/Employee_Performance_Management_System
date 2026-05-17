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
        var positionId = await _currentEmployee.GetPositionIdAsync();
        if (!positionId.HasValue)
            return SuccessResponse.Fail("User position is required.", ErrorType.Forbidden);

        var appraisal = await _uow.Perf.Appraisals.GetAppraisalWithDetailsAsync(dto.Id);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(dto.Id), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

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

    private async Task ResolveAndAddKPIDetailsAsync(Appraisal appraisal, long cycleId)
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
