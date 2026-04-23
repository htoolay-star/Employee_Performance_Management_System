using AutoMapper;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.HR;

namespace EPMS.Domain.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _repo;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeamDto>> GetAllAsync()
    {
        var teams = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<TeamDto?> GetByIdAsync(long id)
    {
        var team = await _repo.GetByIdAsync(id);
        return _mapper.Map<TeamDto>(team);
    }

    public async Task<long> CreateAsync(CreateTeamDto dto)
    {
        if (await _repo.ExistsByNameInDepartmentAsync(dto.Name, dto.DepartmentId))
        {
            throw new InvalidOperationException($"Team with name '{dto.Name}' already exists in this department.");
        }

        var entity = new Team(dto.Name, dto.DepartmentId);
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(long id, TeamDto dto)
    {
        var team = await _repo.GetByIdAsync(id);
        if (team == null) return;

        if (team.Name != dto.Name && await _repo.ExistsByNameInDepartmentAsync(dto.Name, team.DepartmentId))
        {
            throw new InvalidOperationException($"Another team with name '{dto.Name}' already exists in this department.");
        }

        team.Rename(dto.Name);
        
        if (dto.IsActive) team.Reactivate();
        else team.Deactivate();

        _repo.Update(team);
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var team = await _repo.GetByIdAsync(id);
        if (team != null)
        {
            _repo.Delete(team);
            await _repo.SaveChangesAsync();
        }
    }
}
