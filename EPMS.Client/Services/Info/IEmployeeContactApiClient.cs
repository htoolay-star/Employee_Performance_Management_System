using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Refit;

namespace EPMS.Client.Services.Info;

public interface IEmployeeContactApiClient
{
    [Get("/api/employee-contacts")]
    Task<SuccessResponse<IEnumerable<EmployeeContactDto>>> GetAllAsync();

    [Get("/api/employee-contacts/{id}")]
    Task<SuccessResponse<EmployeeContactDto>> GetByIdAsync(long id);

    [Get("/api/employee-contacts/by-employee/{employeePublicId}")]
    Task<SuccessResponse<EmployeeContactDto>> GetByEmployeeIdAsync(Guid employeePublicId);

    [Post("/api/employee-contacts")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateEmployeeContactDto dto);

    [Put("/api/employee-contacts/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEmployeeContactDto dto);

    [Delete("/api/employee-contacts/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
