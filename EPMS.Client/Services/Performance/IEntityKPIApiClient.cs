using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.EntityKPI;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IEntityKPIApiClient
{
    [Get("/api/performance/entity-kpis")]
    Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetAllAsync();

    [Get("/api/performance/entity-kpis/{id}")]
    Task<SuccessResponse<EntityKPIDto>> GetByIdAsync(long id);

    [Get("/api/performance/entity-kpis/by-type/{entityType}")]
    Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetByEntityTypeAsync(string entityType);

    [Get("/api/performance/entity-kpis/by-entity")]
    Task<SuccessResponse<IEnumerable<EntityKPIDto>>> GetByEntityAsync(string entityType, long entityId);

    [Post("/api/performance/entity-kpis")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateEntityKPIDto dto);

    [Put("/api/performance/entity-kpis/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEntityKPIDto dto);

    [Delete("/api/performance/entity-kpis/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
