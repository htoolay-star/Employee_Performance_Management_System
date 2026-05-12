using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.DepartmentDTOs;
using Refit;

namespace EPMS.Client.Services.Hr;

public interface IDepartmentApiClient
{
    [Get("/api/departments/lookup")]
    Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();

    [Get("/api/departments/with-teams")]
    Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetWithTeamsAsync();

    [Get("/api/departments")]
    Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetAllAsync();

    [Get("/api/departments/{id}")]
    Task<SuccessResponse<DepartmentDto>> GetByIdAsync(long id);

    [Post("/api/departments")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateDepartmentDto dto);

    [Put("/api/departments/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateDepartmentDto dto);

    [Delete("/api/departments/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}