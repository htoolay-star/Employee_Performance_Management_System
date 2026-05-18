using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

using Mapster;
namespace EPMS.Domain.Services.Performance;

public class RatingScaleService : IRatingScaleService
{
    private readonly IUnitOfWork _uow;
    
    public RatingScaleService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<SuccessResponse<IEnumerable<RatingScaleDto>>> GetAllAsync()
    {
        var ratingScales = await _uow.Perf.RatingScales.GetAllAsync();
        var dtos = ratingScales.Adapt<IEnumerable<RatingScaleDto>>();
        return SuccessResponse<IEnumerable<RatingScaleDto>>.Ok(dtos, RatingScaleMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<RatingScaleDto>>> GetActiveAsync()
    {
        var ratingScales = await _uow.Perf.RatingScales.GetActiveAsync();
        var dtos = ratingScales.Adapt<IEnumerable<RatingScaleDto>>();
        return SuccessResponse<IEnumerable<RatingScaleDto>>.Ok(dtos, RatingScaleMsg.RetrievedActive);
    }

    public async Task<SuccessResponse<RatingScaleDto>> GetByIdAsync(long id)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByIdAsync(id);

        if (ratingScale == null)
            return SuccessResponse<RatingScaleDto>.Fail(RatingScaleMsg.NotFound(id), ErrorType.NotFound);

        var dto = ratingScale.Adapt<RatingScaleDto>();
        return SuccessResponse<RatingScaleDto>.Ok(dto, RatingScaleMsg.Retrieved);
    }

    public async Task<SuccessResponse<RatingScaleDto>> GetByRatingAsync(int rating)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByRatingAsync(rating);

        if (ratingScale == null)
            return SuccessResponse<RatingScaleDto>.Fail(RatingScaleMsg.NotFoundByRating(rating), ErrorType.NotFound);

        var dto = ratingScale.Adapt<RatingScaleDto>();
        return SuccessResponse<RatingScaleDto>.Ok(dto, RatingScaleMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateRatingScaleDto dto)
    {
        // Validate rating uniqueness
        if (await _uow.Perf.RatingScales.RatingExistsAsync(dto.Rating))
            return SuccessResponse<long>.Fail(string.Format(RatingScaleMsg.DuplicateRating, dto.Rating), ErrorType.Conflict);

        // Validate score bounds
        if (dto.MinScore > dto.MaxScore)
            return SuccessResponse<long>.Fail(RatingScaleMsg.MinGreaterThanMax, ErrorType.Validation);

        var ratingScale = new RatingScale(dto.Rating, dto.Label, dto.MinScore, dto.MaxScore);

        _uow.Perf.RatingScales.Add(ratingScale);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(ratingScale.Id, RatingScaleMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateRatingScaleDto dto)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByIdAsync(id);

        if (ratingScale == null)
            return SuccessResponse.Fail(RatingScaleMsg.NotFound(id), ErrorType.NotFound);

        // Validate score bounds if provided
        if (dto.MinScore.HasValue && dto.MaxScore.HasValue && dto.MinScore.Value > dto.MaxScore.Value)
            return SuccessResponse.Fail(RatingScaleMsg.MinGreaterThanMax, ErrorType.Validation);

        // Update bounds if provided
        if (dto.MinScore.HasValue || dto.MaxScore.HasValue)
        {
            var minScore = dto.MinScore ?? ratingScale.MinScore;
            var maxScore = dto.MaxScore ?? ratingScale.MaxScore;
            ratingScale.UpdateBounds(minScore, maxScore);
        }

        // Update additional details
        ratingScale.UpdateDetails(dto.PromotionEligibility, dto.Description);

        if (dto.IsActive.HasValue)
        {
            if (dto.IsActive.Value) ratingScale.Reactivate();
            else ratingScale.Deactivate();
        }

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(RatingScaleMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByIdAsync(id);

        if (ratingScale == null)
            return SuccessResponse.Fail(RatingScaleMsg.NotFound(id), ErrorType.NotFound);

        _uow.Perf.RatingScales.Delete(ratingScale);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok(RatingScaleMsg.Deleted);
    }

    public async Task<SuccessResponse> RestoreAsync(long id)
        {
            var entity = await _uow.Perf.RatingScales.GetByIdAsync(id);
            if (entity == null)
                return SuccessResponse.Fail(RatingScaleMsg.NotFound(id), ErrorType.NotFound);
            if (!entity.IsDeleted)
                return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            entity.DeletedBy = null;
            _uow.Perf.RatingScales.Update(entity);
            await _uow.CompleteAsync();
            return SuccessResponse.Ok(RatingScaleMsg.Updated);
        }

}