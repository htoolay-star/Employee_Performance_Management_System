using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IQuestionRatingScaleApiClient
{
    [Get("/api/QuestionRatingScales")]
    Task<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>> GetAllAsync();

    [Get("/api/QuestionRatingScales/active")]
    Task<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>> GetActiveAsync();

    [Get("/api/QuestionRatingScales/{id}")]
    Task<SuccessResponse<QuestionRatingScaleDto>> GetByIdAsync(long id);

    [Post("/api/QuestionRatingScales")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateQuestionRatingScaleDto dto);

    [Put("/api/QuestionRatingScales/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateQuestionRatingScaleDto dto);

    [Delete("/api/QuestionRatingScales/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Put("/api/QuestionRatingScales/{id}/deactivate")]
    Task<SuccessResponse> DeactivateAsync(long id);

    [Put("/api/QuestionRatingScales/{id}/reactivate")]
    Task<SuccessResponse> ReactivateAsync(long id);
}
