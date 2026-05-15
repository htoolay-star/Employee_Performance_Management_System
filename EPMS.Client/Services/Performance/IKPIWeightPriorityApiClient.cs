using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IKPIWeightPriorityApiClient
{
    [Get("/api/performance/kpi-weight-priorities")]
    Task<SuccessResponse<IEnumerable<KPIWeightPriorityDto>>> GetAllAsync();

    [Get("/api/performance/kpi-weight-priorities/active")]
    Task<SuccessResponse<IEnumerable<KPIWeightPriorityDto>>> GetActiveAsync();

    [Get("/api/performance/kpi-weight-priorities/{id}")]
    Task<SuccessResponse<KPIWeightPriorityDto>> GetByIdAsync(long id);

    [Post("/api/performance/kpi-weight-priorities")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateKPIWeightPriorityDto dto);

    [Put("/api/performance/kpi-weight-priorities/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateKPIWeightPriorityDto dto);

    [Delete("/api/performance/kpi-weight-priorities/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Post("/api/performance/kpi-weight-priorities/{id}/deactivate")]
    Task<SuccessResponse> DeactivateAsync(long id);

    [Post("/api/performance/kpi-weight-priorities/{id}/reactivate")]
    Task<SuccessResponse> ReactivateAsync(long id);
}
