using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using Refit;

namespace EPMS.Client.Services.Info;

public interface IEmployeeFamilyInfoApiClient
{
    [Get("/api/employee-family-infos")]
    Task<SuccessResponse<IEnumerable<EmployeeFamilyInfoDto>>> GetAllAsync();

    [Get("/api/employee-family-infos/{id}")]
    Task<SuccessResponse<EmployeeFamilyInfoDto>> GetByIdAsync(long id);

    [Get("/api/employee-family-infos/by-employee/{employeePublicId}")]
    Task<SuccessResponse<EmployeeFamilyInfoDto>> GetByEmployeeIdAsync(Guid employeePublicId);

    [Post("/api/employee-family-infos")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateEmployeeFamilyInfoDto dto);

    [Put("/api/employee-family-infos/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEmployeeFamilyInfoDto dto);

    [Delete("/api/employee-family-infos/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
