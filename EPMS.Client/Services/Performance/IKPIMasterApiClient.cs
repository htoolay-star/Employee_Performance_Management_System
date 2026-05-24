using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IKPIMasterApiClient
{
    [Get("/api/performance/kpi-masters/lookup")]
    Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();

    [Get("/api/performance/kpi-masters")]
    Task<SuccessResponse<IEnumerable<KPIMasterDto>>> GetAllAsync();

    [Get("/api/performance/kpi-masters/active")]
    Task<SuccessResponse<IEnumerable<KPIMasterDto>>> GetActiveAsync();

    [Get("/api/performance/kpi-masters/{id}")]
    Task<SuccessResponse<KPIMasterDto>> GetByIdAsync(long id);

    [Post("/api/performance/kpi-masters")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateKPIMasterDto dto);

    [Put("/api/performance/kpi-masters/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateKPIMasterDto dto);

    [Delete("/api/performance/kpi-masters/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Post("/api/performance/kpi-masters/{id}/deactivate")]
    Task<SuccessResponse> DeactivateAsync(long id);

    [Post("/api/performance/kpi-masters/{id}/reactivate")]
    Task<SuccessResponse> ReactivateAsync(long id);
}
