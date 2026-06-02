using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Interface.IService.Hr;
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
            "Code" => "Code",
            "Name" => "Name",
            "DepartmentCode" => "Department.Code",
            "IsActive" => "IsActive",
            _ => "Name"
        };
    }

    public async Task<SuccessResponse<IEnumerable<LookUpDto>>> GetLookupAsync()
    {
        var dtos = await _cacheService.GetOrCreateAsync(
            CacheKeys.Hr.TeamLookups(),
            async () => await _uow.HR.Teams.GetLookupDtoAsync(),
            TimeSpan.FromHours(12)
        );

        return SuccessResponse<IEnumerable<LookUpDto>>.Ok(dtos ?? [], TeamMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<TeamDto>>> GetTeamsByDepartmentIdAsync(long departmentId)
    {
        var teams = await _uow.HR.Teams.GetTeamsByDepartmentAsync(departmentId);

        var dtos = teams.Adapt<IEnumerable<TeamDto>>();

        return SuccessResponse<IEnumerable<TeamDto>>.Ok(dtos, TeamMsg.RetrievedAll);
    }

    public async Task<SuccessResponse<IEnumerable<TeamDto>>> GetAllAsync()
    {
        var teams = await _uow.HR.Teams.GetAllAsync();
        var dtos = teams.Adapt<IEnumerable<TeamDto>>();
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
        if (await _uow.HR.Teams.ExistsByCodeAsync(dto.Code))
            return SuccessResponse<long>.Fail(string.Format(TeamMsg.DuplicateCode, dto.Code.Trim().ToUpperInvariant()), ErrorType.Conflict);

        if (await _uow.HR.Teams.ExistsByNameInDepartmentAsync(dto.Name, dto.DepartmentId))
            return SuccessResponse<long>.Fail(string.Format(TeamMsg.DuplicateName, dto.Name), ErrorType.Conflict);

        var entity = new Team(dto.Code, dto.Name, dto.DepartmentId, dto.Description, dto.LeadTeamId);
        _uow.HR.Teams.Add(entity);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.TeamLookups());
        return SuccessResponse<long>.Ok(entity.Id, TeamMsg.Created);
    }

    public async Task<SuccessResponse> UpdateAsync(long id, UpdateTeamDto dto)
    {
        var team = await _uow.HR.Teams.GetByIdAsync(id);

        if (team == null)
            return SuccessResponse.Fail(TeamMsg.NotFound(id), ErrorType.NotFound);

        if (team.Code != dto.Code.Trim().ToUpperInvariant() && await _uow.HR.Teams.ExistsByCodeAsync(dto.Code, id))
            return SuccessResponse.Fail(string.Format(TeamMsg.DuplicateCode, dto.Code.Trim().ToUpperInvariant()), ErrorType.Conflict);

        // Validate name uniqueness in the target department (current or new)
        var targetDepartmentId = dto.DepartmentId ?? team.DepartmentId;
        if (team.Name != dto.Name && await _uow.HR.Teams.ExistsByNameInDepartmentAsync(dto.Name, targetDepartmentId, id))
            return SuccessResponse.Fail(string.Format(TeamMsg.DuplicateName, dto.Name), ErrorType.Conflict);

        team.Update(dto.Code, dto.Name, dto.Description, dto.LeadTeamId);

        // Handle DepartmentId change
        if (dto.DepartmentId.HasValue && dto.DepartmentId.Value != team.DepartmentId)
        {
            team.ReassignToDepartment(dto.DepartmentId.Value);
        }

        if (dto.IsActive) team.Reactivate();
        else team.Deactivate();

        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.TeamLookups());
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
        await _cacheService.RemoveAsync(CacheKeys.Hr.TeamLookups());
        return SuccessResponse.Ok(TeamMsg.Deleted);
    }
    public async Task<SuccessResponse> RestoreAsync(long id)
    {
        var entity = await _uow.HR.Teams.GetByIdDeletedAsync(id);
        if (entity == null)
            return SuccessResponse.Fail(TeamMsg.NotFound(id), ErrorType.NotFound);
        if (!entity.IsDeleted)
            return SuccessResponse.Fail("Item is not deleted.", ErrorType.Validation);
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = null;
        _uow.HR.Teams.Update(entity);
        await _uow.CompleteAsync();
        await _cacheService.RemoveAsync(CacheKeys.Hr.TeamLookups());
        return SuccessResponse.Ok(TeamMsg.Updated);
    }

}