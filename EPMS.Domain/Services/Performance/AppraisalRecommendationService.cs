using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;
using EPMS.Domain.Interface.IService.Performance;

namespace EPMS.Domain.Services.Performance;

public class AppraisalRecommendationService : IAppraisalRecommendationService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _timeProvider;
    private readonly IMapper _mapper;

    public AppraisalRecommendationService(IUnitOfWork uow, TimeProvider timeProvider, IMapper mapper)
    {
        _uow = uow;
        _timeProvider = timeProvider;
        _mapper = mapper;
    }

    public async Task<SuccessResponse> CreateAsync(CreateAppraisalRecommendationDto dto)
    {
        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(dto.AppraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(dto.AppraisalId), ErrorType.NotFound);

        if (appraisal.IsLocked)
            return SuccessResponse.Fail(AppraisalMsg.AlreadyLocked, ErrorType.Conflict);

        var recommendation = new AppraisalRecommendation(
            dto.AppraisalId, 
            dto.Type, 
            dto.Reason, 
            dto.ProposedValue, 
            dto.Priority);

        _uow.Perf.AppraisalRecommendations.Add(recommendation);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(AppraisalRecommendationMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateAppraisalRecommendationDto dto)
    {
        var recommendation = await _uow.Perf.AppraisalRecommendations.GetByIdWithDetailsAsync(id);
        if (recommendation == null)
            return SuccessResponse.Fail(AppraisalRecommendationMsg.NotFound(id), ErrorType.NotFound);

        if (recommendation.Status != RecommendationStatuses.Pending)
            return SuccessResponse.Fail(AppraisalRecommendationMsg.AlreadyProcessed, ErrorType.Conflict);

        recommendation.UpdateDetails(dto.Type, dto.Reason, dto.ProposedValue, dto.Priority);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalRecommendationMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var recommendation = await _uow.Perf.AppraisalRecommendations.GetByIdAsync(id);
        if (recommendation == null)
            return SuccessResponse.Fail(AppraisalRecommendationMsg.NotFound(id), ErrorType.NotFound);

        if (recommendation.Status != RecommendationStatuses.Pending)
            return SuccessResponse.Fail(AppraisalRecommendationMsg.CannotModify, ErrorType.Conflict);

        recommendation.IsDeleted = true;
        recommendation.DeletedAt = _timeProvider.GetUtcNow();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(AppraisalRecommendationMsg.Deleted);
    }

    public async Task<SuccessResponse> GetByIdAsync(long id)
    {
        var recommendation = await _uow.Perf.AppraisalRecommendations.GetByIdWithDetailsAsync(id);
        if (recommendation == null)
            return SuccessResponse.Fail(AppraisalRecommendationMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<AppraisalRecommendationDto>(recommendation);
        return SuccessResponse<AppraisalRecommendationDto>.Ok(dto, AppraisalRecommendationMsg.Retrieved);
    }

    public async Task<SuccessResponse> GetAllAsync()
    {
        var recommendations = await _uow.Perf.AppraisalRecommendations.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<AppraisalRecommendationDto>>(recommendations.Where(r => !r.IsDeleted));
        return SuccessResponse<IEnumerable<AppraisalRecommendationDto>>.Ok(dtos, AppraisalRecommendationMsg.RetrievedAll);
    }

    public async Task<SuccessResponse> GetByAppraisalIdAsync(long appraisalId)
    {
        var appraisal = await _uow.Perf.Appraisals.GetByIdAsync(appraisalId);
        if (appraisal == null)
            return SuccessResponse.Fail(AppraisalMsg.NotFound(appraisalId), ErrorType.NotFound);

        var recommendations = await _uow.Perf.AppraisalRecommendations.GetByAppraisalIdAsync(appraisalId);
        var dtos = _mapper.Map<IEnumerable<AppraisalRecommendationDto>>(recommendations.Where(r => !r.IsDeleted));
        return SuccessResponse<IEnumerable<AppraisalRecommendationDto>>.Ok(dtos, AppraisalRecommendationMsg.RetrievedByAppraisal);
    }

    public async Task<SuccessResponse> ApproveAsync(long id, long hrAdminId, string? comments)
    {
        var recommendation = await _uow.Perf.AppraisalRecommendations.GetByIdWithDetailsAsync(id);
        if (recommendation == null)
            return SuccessResponse.Fail(AppraisalRecommendationMsg.NotFound(id), ErrorType.NotFound);

        try
        {
            recommendation.Approve(hrAdminId, comments, _timeProvider);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok(AppraisalRecommendationMsg.Approved);
        }
        catch (InvalidOperationException ex)
        {
            return SuccessResponse.Fail(ex.Message, ErrorType.Conflict);
        }
    }

    public async Task<SuccessResponse> RejectAsync(long id, long hrAdminId, string reason)
    {
        var recommendation = await _uow.Perf.AppraisalRecommendations.GetByIdWithDetailsAsync(id);
        if (recommendation == null)
            return SuccessResponse.Fail(AppraisalRecommendationMsg.NotFound(id), ErrorType.NotFound);

        try
        {
            recommendation.Reject(hrAdminId, reason, _timeProvider);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok(AppraisalRecommendationMsg.Rejected);
        }
        catch (InvalidOperationException ex)
        {
            return SuccessResponse.Fail(ex.Message, ErrorType.Conflict);
        }
    }
}
