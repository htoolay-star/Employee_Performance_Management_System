using EPMS.Api.Authorization;
using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Hr;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Features.Teams;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ApiControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HasPermission("TEAMS.VIEW")]
    [HttpGet("lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<LookUpDto>>>> GetLookup()
    {
        var result = await _teamService.GetLookupAsync();
        return HandleResult(result);
    }

    [HasPermission("TEAMS.VIEW")]
    [HttpGet("by-department/{departmentId:long}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<TeamDto>>>> GetByDepartment(long departmentId)
    {
        var result = await _teamService.GetTeamsByDepartmentIdAsync(departmentId);
        return HandleResult(result);
    }

    [HasPermission("TEAMS.VIEW")]
    [HttpGet("paged")]
    public async Task<ActionResult<SuccessResponse<PaginatedResponse<TeamGridItemDto>>>> GetPaged([FromQuery] TeamQueryParameters parameters)
    {
        var result = await _teamService.GetPagedAsync(parameters);
        return HandleResult(result);
    }

    [HasPermission("TEAMS.VIEW")]
    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<TeamDto>>>> GetAll()
    {
        var result = await _teamService.GetAllAsync();
        return HandleResult(result);
    }

    [HasPermission("TEAMS.VIEW")]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<TeamDto>>> GetById(long id)
    {
        var result = await _teamService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HasPermission("TEAMS.CREATE")]
    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateTeamDto dto)
    {
        var result = await _teamService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HasPermission("TEAMS.EDIT")]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateTeamDto dto)
    {
        var result = await _teamService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HasPermission("TEAMS.DELETE")]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _teamService.DeleteAsync(id);
        return HandleResult(result);
    }
        [HasPermission("TEAMS.DELETE")]
        [HttpPost("{id:long}/restore")]
        public async Task<ActionResult<SuccessResponse>> Restore(long id)
        {
            var result = await _teamService.RestoreAsync(id);
            return HandleResult(result);
        }
}