using AutoMapper;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interfaces;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Enums;
using EPMS.Shared.Features.Teams;
using Mapster;
using static EPMS.Shared.Constants.ServiceResponseMessages;

namespace EPMS.Domain.Services.Hr;

public class TeamService : ITeamService
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cacheService;

    public TeamService(IUnitOfWork uow, ICacheService cacheService)
    {
        _uow = uow;
        _cacheService = cacheService;
    }

    public async Task<SuccessResponse<PaginatedResponse<TeamGridItemDto>>> GetPagedAsync(TeamQueryParameters parameters)
    {
        var entitySortColumn = GetMappedSortColumn(parameters.OrderBy);

        var (items, totalCount) = await _uow.HR.Teams.GetPagedAsync(parameters, entitySortColumn);

        var response = new PaginatedResponse<TeamGridItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };

        return SuccessResponse<PaginatedResponse<TeamGridItemDto>>.Ok(response, TeamMsg.RetrievedAll);
    }

    private static string GetMappedSortColumn(string? orderBy)
    {
        return orderBy switch
        {
            "Name" => "Name",
            "Department" => "Department.Name",
            "IsActive" => "IsActive",
            _ => "Name"
        };
    }

    public async Task<SuccessResponse<IEnumerable<TeamLookupDto>>> GetLookupAsync()
    {
        var cachedAllTeams = await _cacheService.GetAsync<IEnumerable<TeamDto>>(CacheKeys.Hr.AllTeams());

        if (cachedAllTeams != null)
        {
            var lookupFromCache = cachedAllTeams.Select(x => new TeamLookupDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            });
            return SuccessResponse<IEnumerable<TeamLookupDto>>.Ok(lookupFromCache, TeamMsg.RetrievedAll);
        }

        var tuples = await _uow.HR.Teams.GetLookupAsync();

        var dtos = tuples.Select(t => new TeamLookupDto
        {
            Id = t.Id,
            Name = t.Name,
            IsActive = t.IsActive
        }).ToList();

        return SuccessResponse<IEnumerable<TeamLookupDto>>.Ok(dtos, TeamMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<TeamDto>>> GetTeamsByDepartmentIdAsync(long departmentId)
    {
        var teams = await _uow.HR.Teams.GetTeamsByDepartmentAsync(departmentId);

        var dtos = teams.Adapt<IEnumerable<TeamDto>>();

        return SuccessResponse<IEnumerable<TeamDto>>.Ok(dtos, TeamMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<TeamDto>>> GetAllAsync()
    {
        var dtos = await _cacheService.GetOrCreateAsync(CacheKeys.Hr.AllTeams(), async () =>
        {
            var teams = await _uow.HR.Teams.GetAllAsync();
            return teams.Adapt<IEnumerable<TeamDto>>();
        });
        return SuccessResponse<IEnumerable<TeamDto>>.Ok(dtos, TeamMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<TeamDto>> GetByIdAsync(long id)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            return SuccessResponse<TeamDto>.Fail(TeamMsg.NotFound(id), ErrorType.NotFound);

        var dto = team.Adapt<TeamDto>();
        return SuccessResponse<TeamDto>.Ok(dto, TeamMsg.Retrieved);
    }

    public async Task<SuccessResponse<long>> CreateAsync(CreateTeamDto dto)
    {
        if (await _uow.HR.Teams.ExistsByNameInDepartmentAsync(dto.Name, dto.DepartmentId))
            return SuccessResponse<long>.Fail(string.Format(TeamMsg.DuplicateName, dto.Name), ErrorType.Conflict);

        var entity = new Team(dto.Name, dto.DepartmentId);
        _uow.HR.Teams.Add(entity);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllTeams());
        return SuccessResponse<long>.Ok(entity.Id, TeamMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateTeamDto dto)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            return SuccessResponse.Fail(TeamMsg.NotFound(id), ErrorType.NotFound);

        // Validate name uniqueness in the target department (current or new)
        var targetDepartmentId = dto.DepartmentId ?? team.DepartmentId;
        if (team.Name != dto.Name && await _uow.HR.Teams.ExistsByNameInDepartmentAsync(dto.Name, targetDepartmentId, id))
            return SuccessResponse.Fail(string.Format(TeamMsg.DuplicateName, dto.Name), ErrorType.Conflict);

        team.Rename(dto.Name);
        
        // Handle DepartmentId change
        if (dto.DepartmentId.HasValue && dto.DepartmentId.Value != team.DepartmentId)
        {
            team.ReassignToDepartment(dto.DepartmentId.Value);
        }

        if (dto.IsActive) team.Reactivate();
        else team.Deactivate();

        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllTeams());
        return SuccessResponse.Ok(TeamMsg.Updated);
    }

    public async Task<SuccessResponse> DeleteAsync(long id)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            return SuccessResponse.Fail(TeamMsg.NotFound(id), ErrorType.NotFound);

        if (await _uow.Info.EmployeeEmployments.AnyAsync(e => e.TeamId == id))
            return SuccessResponse.Fail(TeamMsg.InUse(id), ErrorType.Conflict);

        _uow.HR.Teams.Delete(team);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.AllTeams());
        return SuccessResponse.Ok(TeamMsg.Deleted);
    }
}
