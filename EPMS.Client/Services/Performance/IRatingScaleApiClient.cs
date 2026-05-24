using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IRatingScaleApiClient
{
    [Get("/api/performance/rating-scales")]
    Task<SuccessResponse<IEnumerable<RatingScaleDto>>> GetAllAsync();

    [Get("/api/performance/rating-scales/active")]
    Task<SuccessResponse<IEnumerable<RatingScaleDto>>> GetActiveAsync();

    [Get("/api/performance/rating-scales/{id}")]
    Task<SuccessResponse<RatingScaleDto>> GetByIdAsync(long id);

    [Get("/api/performance/rating-scales/by-rating/{rating}")]
    Task<SuccessResponse<RatingScaleDto>> GetByRatingAsync(int rating);

    [Post("/api/performance/rating-scales")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateRatingScaleDto dto);

    [Put("/api/performance/rating-scales/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateRatingScaleDto dto);

    [Delete("/api/performance/rating-scales/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
