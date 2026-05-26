using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IOneOnOneMeetingApiClient
{
    [Get("/api/performance/one-on-one-meetings")]
    Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetAllAsync();

    [Get("/api/performance/one-on-one-meetings/upcoming")]
    Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetUpcomingAsync();

    [Get("/api/performance/one-on-one-meetings/{id}")]
    Task<SuccessResponse<OneOnOneMeetingDto>> GetByIdAsync(long id);

    [Get("/api/performance/one-on-one-meetings/employee/{employeeId}")]
    Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByEmployeeAsync(long employeeId);

    [Get("/api/performance/one-on-one-meetings/manager/{managerId}")]
    Task<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>> GetByManagerAsync(long managerId);

    [Post("/api/performance/one-on-one-meetings")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateOneOnOneMeetingDto dto);

    [Put("/api/performance/one-on-one-meetings/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateOneOnOneMeetingDto dto);

    [Delete("/api/performance/one-on-one-meetings/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);

    [Post("/api/performance/one-on-one-meetings/{id}/complete")]
    Task<SuccessResponse> CompleteAsync(long id, [Body] CompleteMeetingDto dto);

    [Post("/api/performance/one-on-one-meetings/{id}/cancel")]
    Task<SuccessResponse> CancelAsync(long id);

    [Post("/api/performance/one-on-one-meetings/{id}/acknowledge")]
    Task<SuccessResponse> AcknowledgeAsync(long id);
}
