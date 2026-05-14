using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IAppraisalCycleApiClient
{
    [Get("/api/performance/appraisal-cycles")]
    Task<SuccessResponse<IEnumerable<AppraisalCycleDto>>> GetAllAsync();

    [Get("/api/performance/appraisal-cycles/active")]
    Task<SuccessResponse<IEnumerable<AppraisalCycleDto>>> GetActiveAsync();

    [Get("/api/performance/appraisal-cycles/{id}")]
    Task<SuccessResponse<AppraisalCycleDto>> GetByIdAsync(long id);

    [Post("/api/performance/appraisal-cycles")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateAppraisalCycleDto dto);

    [Put("/api/performance/appraisal-cycles/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateAppraisalCycleDto dto);

    [Delete("/api/performance/appraisal-cycles/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Post("/api/performance/appraisal-cycles/{id}/lock")]
    Task<SuccessResponse> LockAsync(long id);

    [Post("/api/performance/appraisal-cycles/{id}/deactivate")]
    Task<SuccessResponse> DeactivateAsync(long id);

    [Post("/api/performance/appraisal-cycles/{id}/reactivate")]
    Task<SuccessResponse> ReactivateAsync(long id);
}
