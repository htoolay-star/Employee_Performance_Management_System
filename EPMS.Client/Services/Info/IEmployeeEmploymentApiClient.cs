using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Refit;

namespace EPMS.Client.Services.Info;

public interface IEmployeeEmploymentApiClient
{
    [Get("/api/employee-employments")]
    Task<SuccessResponse<IEnumerable<EmployeeEmploymentDto>>> GetAllAsync();

    [Get("/api/employee-employments/{id}")]
    Task<SuccessResponse<EmployeeEmploymentDto>> GetByIdAsync(long id);

    [Get("/api/employee-employments/by-employee/{employeePublicId}")]
    Task<SuccessResponse<EmployeeEmploymentDto>> GetByEmployeeIdAsync(Guid employeePublicId);

    [Post("/api/employee-employments")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateEmployeeEmploymentDto dto);

    [Put("/api/employee-employments/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEmployeeEmploymentDto dto);

    [Delete("/api/employee-employments/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
