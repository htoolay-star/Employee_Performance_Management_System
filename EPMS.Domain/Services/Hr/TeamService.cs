using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.HR;

namespace EPMS.Domain.Services.Hr;

public class TeamService : ITeamService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public TeamService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<IEnumerable<TeamDto>> GetAllAsync()
    {
        var teams = await _uow.HR.Teams.GetAllAsync();
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<TeamDto?> GetByIdAsync(long id)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            throw new KeyNotFoundException($"Team with ID '{id}' was not found.");

        return _mapper.Map<TeamDto>(team);
    }

    public async Task<long> CreateAsync(CreateTeamDto dto)
    {
        if (await _uow.HR.Teams.ExistsByNameInDepartmentAsync(dto.Name, dto.DepartmentId))
        {
            throw new InvalidOperationException($"Team with name '{dto.Name}' already exists in this department.");
        }

        var entity = new Team(dto.Name, dto.DepartmentId);
        _uow.HR.Teams.Add(entity);
        await _uow.CompleteAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(long id, UpdateTeamDto dto)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            throw new KeyNotFoundException($"Team with ID '{id}' was not found.");

        if (team.Name != dto.Name && await _uow.HR.Teams.ExistsByNameInDepartmentAsync(dto.Name, team.DepartmentId))
        {
            throw new InvalidOperationException($"Another team with name '{dto.Name}' already exists in this department.");
        }

        team.Rename(dto.Name);
        
        if (dto.IsActive) team.Reactivate();
        else team.Deactivate();

        await _uow.CompleteAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            throw new KeyNotFoundException($"Team with ID '{id}' was not found.");

        if (team != null)
        {
            _uow.HR.Teams.Delete(team);
            await _uow.CompleteAsync();
        }
    }
}
