using EPMS.Shared.DTOs.HR;

namespace EPMS.Domain.Interfaces;

public interface ITeamService
{
    Task<IEnumerable<TeamDto>> GetAllAsync();
    Task<TeamDto?> GetByIdAsync(long id);
    Task<long> CreateAsync(CreateTeamDto dto);
    Task UpdateAsync(long id, UpdateTeamDto dto);
    Task DeleteAsync(long id);
}
