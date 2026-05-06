using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TeamDTOs;

namespace EPMS.Domain.Interfaces;

public interface ITeamService
{
    Task<SuccessResponse<IEnumerable<TeamDto>>> GetAllAsync();
    Task<SuccessResponse<TeamDto>> GetByIdAsync(long id);
    Task<SuccessResponse<long>> CreateAsync(CreateTeamDto dto);
    Task<SuccessResponse> UpdateAsync(long id, UpdateTeamDto dto);
    Task<SuccessResponse> DeleteAsync(long id);
}
