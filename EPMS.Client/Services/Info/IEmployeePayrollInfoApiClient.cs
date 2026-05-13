using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Refit;

namespace EPMS.Client.Services.Info;

public interface IEmployeePayrollInfoApiClient
{
    [Get("/api/employee-payroll-infos")]
    Task<SuccessResponse<IEnumerable<EmployeePayrollInfoDto>>> GetAllAsync();

    [Get("/api/employee-payroll-infos/{id}")]
    Task<SuccessResponse<EmployeePayrollInfoDto>> GetByIdAsync(long id);

    [Get("/api/employee-payroll-infos/by-employee/{employeePublicId}")]
    Task<SuccessResponse<EmployeePayrollInfoDto>> GetByEmployeeIdAsync(Guid employeePublicId);

    [Post("/api/employee-payroll-infos")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateEmployeePayrollInfoDto dto);

    [Put("/api/employee-payroll-infos/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEmployeePayrollInfoDto dto);

    [Delete("/api/employee-payroll-infos/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
