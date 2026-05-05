using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.TeamDTOs;

namespace EPMS.Domain.Interfaces;

public interface IDepartmentService
{
    Task<SuccessResponse<IEnumerable<DepartmentDto>>> GetAllAsync();
    Task<SuccessResponse<DepartmentDto>> GetByIdAsync(long id);
    Task<SuccessResponse<long>> CreateAsync(CreateDepartmentDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateDepartmentDto dto);
    Task<SuccessResponse> DeleteAsync(long id);

    // Team management methods
    Task<SuccessResponse<IEnumerable<TeamDto>>> GetTeamsForDepartmentAsync(long departmentId);
    Task<SuccessResponse> AddTeamToDepartmentAsync(long departmentId, string teamName);
    Task<SuccessResponse> RemoveTeamFromDepartmentAsync(long departmentId, long teamId);
}