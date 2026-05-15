using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IKPIMasterApiClient
{
    [Get("/api/performance/kpi-masters/active")]
    Task<SuccessResponse<IEnumerable<KPIMasterDto>>> GetActiveAsync();
}
