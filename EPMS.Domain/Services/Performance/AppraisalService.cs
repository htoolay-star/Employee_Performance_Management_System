using AutoMapper;
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

namespace EPMS.Domain.Services.Performance;

public class AppraisalService : IAppraisalService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _timeProvider;
    private readonly IMapper _mapper;
    private readonly ICurrentEmployeeContextService _currentEmployee;

    public AppraisalService(
        IUnitOfWork uow,
        TimeProvider timeProvider,
        IMapper mapper,
        ICurrentEmployeeContextService currentEmployee)
    {
        _uow = uow;
        _timeProvider = timeProvider;
        _mapper = mapper;
        _currentEmployee = currentEmployee;
    }

    public async Task<SuccessResponse> CreateAsync(CreateAppraisalDto dto)
    {
        var employee = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.EmployeeId);
        if (employee == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(dto.EmployeeId), ErrorType.NotFound);

        var cycle = await _uow.Perf.AppraisalCycles.GetByIdAsync(dto.CycleId);
        if (cycle == null)
            return SuccessResponse.Fail(AppraisalCycleMsg.NotFound(dto.CycleId), ErrorType.NotFound);

        var appraiser = await _uow.Info.EmployeeProfiles.GetByIdAsync(dto.AppraiserId);
        if (appraiser == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(dto.AppraiserId), ErrorType.NotFound);

        var hasExisting = await _uow.Perf.Appraisals.HasAlreadySubmittedAsync(
            dto.EmployeeId, dto.AppraiserId, (int)dto.CycleId, dto.EvaluatorRole);
        if (hasExisting)
            return SuccessResponse.Fail(AppraisalMsg.DuplicateEntry, ErrorType.Conflict);

        var appraisal = new Appraisal(dto.EmployeeId, dto.CycleId, dto.AppraiserId, dto.EvaluatorRole);

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

        var dto = _mapper.Map<AppraisalDto>(appraisal);
        return SuccessResponse<AppraisalDto>.Ok(dto, AppraisalMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAllAsync()
    {
        var appraisals = await _uow.Perf.Appraisals.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<AppraisalDto>>(appraisals.Where(a => !a.IsDeleted));
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByEmployeeIdAsync(long employeeId)
    {
        var employee = await _uow.Info.EmployeeProfiles.GetByIdAsync(employeeId);
        if (employee == null)
            return SuccessResponse.Fail(EmployeeProfileMsg.NotFound(employeeId), ErrorType.NotFound);

        var appraisals = await _uow.Perf.Appraisals.GetEmployeeAppraisalsAsync(employeeId, 0);
        var dtos = _mapper.Map<IEnumerable<AppraisalDto>>(appraisals.Where(a => !a.IsDeleted));
        return SuccessResponse<IEnumerable<AppraisalDto>>.Ok(dtos, AppraisalMsg.RetrievedByEmployee);
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
                (detailDto.KPIId.HasValue && d.KPIId == detailDto.KPIId) ||
                (detailDto.QuestionId.HasValue && d.QuestionId == detailDto.QuestionId));

            if (detail != null)
            {
                detail.Evaluate(detailDto.ActualValue, detailDto.Rating, detailDto.Comment);
            }
        }

        var currentTotalScore = appraisal.Details.Sum(d => d.WeightedScore);
        var scales = await _uow.Perf.RatingScales.GetAllAsync();
        var matchingScale = scales.FirstOrDefault(s =>
            currentTotalScore >= s.MinScore && currentTotalScore <= s.MaxScore);

        if (matchingScale != null)
        {
            appraisal.CalculateTotalScore(matchingScale);
        }

        _uow.Perf.Appraisals.Update(appraisal);
        await _uow.CompleteAsync();

        var response = new AppraisalResponseDto
        {
            Id = appraisal.Id,
            TotalScore = appraisal.TotalScore ?? 0,
            Grade = appraisal.RatingLabel ?? "N/A"
        };

        return SuccessResponse<AppraisalResponseDto>.Ok(response, AppraisalMsg.Submitted);
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

        appraisal.FinalizeAppraisal(_timeProvider);

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
}