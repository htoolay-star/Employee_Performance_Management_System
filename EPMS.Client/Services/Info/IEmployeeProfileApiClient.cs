using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Features.EmployeeProfiles;
using Refit;

namespace EPMS.Client.Services.Info;

public interface IEmployeeProfileApiClient
{
    [Get("/api/employee-profiles/lookup")]
    Task<SuccessResponse<IEnumerable<EmployeeLookupDto>>> GetLookupAsync();

    [Get("/api/employee-profiles")]
    Task<SuccessResponse<IEnumerable<EmployeeProfileDto>>> GetAllAsync();

    [Get("/api/employee-profiles/paged")]
    Task<SuccessResponse<PaginatedResponse<EmployeeProfileGridItemDto>>> GetPagedAsync([Query] EmployeeProfileQueryParameters parameters);

    [Get("/api/employee-profiles/{id}")]
    Task<SuccessResponse<EmployeeProfileDto>> GetByIdAsync(long id);

    [Post("/api/employee-profiles")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateEmployeeProfileDto dto);

    [Put("/api/employee-profiles/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateEmployeeProfileDto dto);

    [Delete("/api/employee-profiles/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}
