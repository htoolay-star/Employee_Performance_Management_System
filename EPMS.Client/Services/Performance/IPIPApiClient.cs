using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IPIPApiClient
{
    [Get("/api/performance/pips")]
    Task<SuccessResponse<IEnumerable<PIPDto>>> GetAllAsync();

    [Get("/api/performance/pips/my")]
    Task<SuccessResponse<IEnumerable<PIPDto>>> GetMyPIPsAsync();

    [Get("/api/performance/pips/active")]
    Task<SuccessResponse<IEnumerable<PIPDto>>> GetActiveAsync();

    [Get("/api/performance/pips/employee/{employeeId}")]
    Task<SuccessResponse<IEnumerable<PIPDto>>> GetByEmployeeAsync(long employeeId);

    [Get("/api/performance/pips/manager/{managerId}")]
    Task<SuccessResponse<IEnumerable<PIPDto>>> GetByManagerAsync(long managerId);

    [Get("/api/performance/pips/{id}")]
    Task<SuccessResponse<PIPDto>> GetByIdAsync(long id);

    [Post("/api/performance/pips")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreatePIPDto dto);

    [Put("/api/performance/pips/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdatePIPDto dto);

    [Delete("/api/performance/pips/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Post("/api/performance/pips/{id}/conclude")]
    Task<SuccessResponse> ConcludeAsync(long id, [Body] ConcludePIPDto dto);

    [Post("/api/performance/pips/{id}/extend")]
    Task<SuccessResponse> ExtendAsync(long id, [Body] ExtendPIPDto dto);
}
