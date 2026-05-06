using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Performance;

public class RatingScaleService : IRatingScaleService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RatingScaleService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<SuccessResponse<IEnumerable<RatingScaleDto>>> GetAllAsync()
    {
        var ratingScales = await _uow.Perf.RatingScales.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<RatingScaleDto>>(ratingScales);
        return SuccessResponse<IEnumerable<RatingScaleDto>>.Ok(dtos, "Rating scales retrieved successfully.");
    }

    public async Task<SuccessResponse<IEnumerable<RatingScaleDto>>> GetActiveAsync()
    {
        var ratingScales = await _uow.Perf.RatingScales.GetActiveAsync();
        var dtos = _mapper.Map<IEnumerable<RatingScaleDto>>(ratingScales);
        return SuccessResponse<IEnumerable<RatingScaleDto>>.Ok(dtos, "Active rating scales retrieved successfully.");
    }

    public async Task<SuccessResponse<RatingScaleDto>> GetByIdAsync(long id)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByIdAsync(id);

        if (ratingScale == null)
            return SuccessResponse<RatingScaleDto>.Fail($"Rating scale with ID '{id}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<RatingScaleDto>(ratingScale);
        return SuccessResponse<RatingScaleDto>.Ok(dto, "Rating scale retrieved successfully.");
    }

    public async Task<SuccessResponse<RatingScaleDto>> GetByRatingAsync(int rating)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByRatingAsync(rating);

        if (ratingScale == null)
            return SuccessResponse<RatingScaleDto>.Fail($"Rating scale with rating '{rating}' was not found.", ErrorType.NotFound);

        var dto = _mapper.Map<RatingScaleDto>(ratingScale);
        return SuccessResponse<RatingScaleDto>.Ok(dto, "Rating scale retrieved successfully.");
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateRatingScaleDto dto)
    {
        // Validate rating uniqueness
        if (await _uow.Perf.RatingScales.RatingExistsAsync(dto.Rating))
            return SuccessResponse<long>.Fail($"Rating scale with rating '{dto.Rating}' already exists.", ErrorType.Conflict);

        // Validate score bounds
        if (dto.MinScore > dto.MaxScore)
            return SuccessResponse<long>.Fail("Minimum score cannot be greater than maximum score.", ErrorType.Validation);

        var ratingScale = new RatingScale(dto.Rating, dto.Label, dto.MinScore, dto.MaxScore);

        _uow.Perf.RatingScales.Add(ratingScale);
        await _uow.CompleteAsync();

        return SuccessResponse<long>.Ok(ratingScale.Id, "Rating scale created successfully.");
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateRatingScaleDto dto)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByIdAsync(id);

        if (ratingScale == null)
            return SuccessResponse.Fail($"Rating scale with ID '{id}' was not found.", ErrorType.NotFound);

        // Validate score bounds if provided
        if (dto.MinScore.HasValue && dto.MaxScore.HasValue && dto.MinScore.Value > dto.MaxScore.Value)
            return SuccessResponse.Fail("Minimum score cannot be greater than maximum score.", ErrorType.Validation);

        // Update bounds if provided
        if (dto.MinScore.HasValue || dto.MaxScore.HasValue)
        {
            var minScore = dto.MinScore ?? ratingScale.MinScore;
            var maxScore = dto.MaxScore ?? ratingScale.MaxScore;
            ratingScale.UpdateBounds(minScore, maxScore);
        }

        // Update additional details
        ratingScale.UpdateDetails(dto.PerformanceLevel, dto.PromotionEligibility, dto.Description);

        await _uow.CompleteAsync();
        return SuccessResponse.Ok("Rating scale updated successfully.");
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByIdAsync(id);

        if (ratingScale == null)
            return SuccessResponse.Fail($"Rating scale with ID '{id}' was not found.", ErrorType.NotFound);

        _uow.Perf.RatingScales.Delete(ratingScale);
        await _uow.CompleteAsync();

        return SuccessResponse.Ok("Rating scale deleted successfully.");
    }

    public async Task<SuccessResponse> DeactivateAsync(long id)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByIdAsync(id);

        if (ratingScale == null)
            return SuccessResponse.Fail($"Rating scale with ID '{id}' was not found.", ErrorType.NotFound);

        ratingScale.Deactivate();
        await _uow.CompleteAsync();

        return SuccessResponse.Ok("Rating scale deactivated successfully.");
    }

    public async Task<SuccessResponse> ReactivateAsync(long id)
    {
        var ratingScale = await _uow.Perf.RatingScales.GetByIdAsync(id);

        if (ratingScale == null)
            return SuccessResponse.Fail($"Rating scale with ID '{id}' was not found.", ErrorType.NotFound);

        ratingScale.Reactivate();
        await _uow.CompleteAsync();

        return SuccessResponse.Ok("Rating scale reactivated successfully.");
    }
}
