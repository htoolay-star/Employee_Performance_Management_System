using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Features.Teams;
using Refit;

namespace EPMS.Client.Services.Hr;

public interface ITeamApiClient
{
    [Get("/api/teams/lookup")]
    Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync();

    [Get("/api/teams/by-department/{departmentId}")]
    Task<SuccessResponse<IEnumerable<TeamDto>>> GetByDepartmentAsync(long departmentId);

    [Get("/api/teams/paged")]
    Task<SuccessResponse<PaginatedResponse<TeamGridItemDto>>> GetPagedAsync([Query] TeamQueryParameters parameters);

    [Get("/api/teams")]
    Task<SuccessResponse<IEnumerable<TeamDto>>> GetAllAsync();

    [Get("/api/teams/{id}")]
    Task<SuccessResponse<TeamDto>> GetByIdAsync(long id);

    [Post("/api/teams")]
    Task<SuccessResponse<long>> CreateAsync([Body] CreateTeamDto dto);

    [Put("/api/teams/{id}")]
    Task<SuccessResponse> UpdateAsync(long id, [Body] UpdateTeamDto dto);

    [Delete("/api/teams/{id}")]
    Task<SuccessResponse> DeleteAsync(long id);
}