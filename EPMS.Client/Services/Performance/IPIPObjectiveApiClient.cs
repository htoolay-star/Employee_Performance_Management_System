using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IPIPObjectiveApiClient
{
    [Get("/api/PIPObjectives")]
    Task<SuccessResponse<IEnumerable<PIPObjectiveDto>>> GetAllAsync();

    [Get("/api/PIPObjectives/{id}")]
    Task<SuccessResponse<PIPObjectiveDto>> GetByIdAsync(long id);

    [Get("/api/PIPObjectives/pip/{pipId}")]
    Task<SuccessResponse<IEnumerable<PIPObjectiveDto>>> GetByPIPIdAsync(long pipId);

    [Post("/api/PIPObjectives")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreatePIPObjectiveDto dto);

    [Put("/api/PIPObjectives/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdatePIPObjectiveDto dto);

    [Delete("/api/PIPObjectives/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
