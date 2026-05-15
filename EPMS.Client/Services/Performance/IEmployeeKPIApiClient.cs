using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.EmployeeKPI;
using Refit;

namespace EPMS.Client.Services.Performance;

public interface IEmployeeKPIApiClient
{
    [Get("/api/performance/employee-kpis")]
    Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetAllAsync();

    [Get("/api/performance/employee-kpis/{id}")]
    Task<SuccessResponse<EmployeeKPIDto>> GetByIdAsync(long id);

    [Get("/api/performance/employee-kpis/by-employee/{employeeId}/cycle/{cycleId}")]
    Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetByEmployeeAndCycleAsync(long employeeId, long cycleId);

    [Get("/api/performance/employee-kpis/by-cycle/{cycleId}")]
    Task<SuccessResponse<IEnumerable<EmployeeKPIDto>>> GetByCycleAsync(long cycleId);

    [Post("/api/performance/employee-kpis")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateEmployeeKPIDto dto);

    [Put("/api/performance/employee-kpis/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEmployeeKPIDto dto);

    [Delete("/api/performance/employee-kpis/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
