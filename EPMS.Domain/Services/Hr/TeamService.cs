using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interfaces;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

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

    public async Task<SuccessResponse<IEnumerable<TeamDto>>> GetAllAsync()
    {
        var teams = await _uow.HR.Teams.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<TeamDto>>(teams);
        return SuccessResponse<IEnumerable<TeamDto>>.Ok(dtos, TeamMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<TeamDto>> GetByIdAsync(long id)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            return SuccessResponse<TeamDto>.Fail(TeamMsg.NotFound(id), ErrorType.NotFound);

        var dto = _mapper.Map<TeamDto>(team);
        return SuccessResponse<TeamDto>.Ok(dto, TeamMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateTeamDto dto)
    {
        if (await _uow.HR.Teams.ExistsByNameInDepartmentAsync(dto.Name, dto.DepartmentId))
            return SuccessResponse<long>.Fail(string.Format(TeamMsg.DuplicateName, dto.Name), ErrorType.Conflict);

        var entity = new Team(dto.Name, dto.DepartmentId);
        _uow.HR.Teams.Add(entity);
        await _uow.CompleteAsync();
        return SuccessResponse<long>.Ok(entity.Id, TeamMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateTeamDto dto)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            return SuccessResponse.Fail(TeamMsg.NotFound(id), ErrorType.NotFound);

        if (team.Name != dto.Name && await _uow.HR.Teams.ExistsByNameInDepartmentAsync(dto.Name, team.DepartmentId))
            return SuccessResponse.Fail(string.Format(TeamMsg.DuplicateName, dto.Name), ErrorType.Conflict);

        team.Rename(dto.Name);
        
        if (dto.IsActive) team.Reactivate();
        else team.Deactivate();

        await _uow.CompleteAsync();
        return SuccessResponse.Ok(TeamMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            return SuccessResponse.Fail(TeamMsg.NotFound(id), ErrorType.NotFound);

        _uow.HR.Teams.Delete(team);
        await _uow.CompleteAsync();
        return SuccessResponse.Ok(TeamMsg.Deleted);
    }
}
