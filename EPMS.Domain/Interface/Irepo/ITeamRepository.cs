using AutoMapper;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.HR;

public interface ITeamService
{
    Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
    Task<TeamDto> CreateTeamAsync(CreateTeamDto dto);
}

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepo;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepo, IMapper mapper)
    {
        _teamRepo = teamRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
    {
        var teams = await _teamRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamDto dto)
    {
        
        var team = new Team(dto.Name, dto.DepartmentId);
        await _teamRepo.AddAsync(team);
        await _teamRepo.SaveChangesAsync();

        return _mapper.Map<TeamDto>(team);
    }
}