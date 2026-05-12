using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Hr;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Features.Teams;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<LookUpDto>>>> GetLookup()
    {
        var result = await _teamService.GetLookupAsync();
        return HandleResult(result);
    }

    [HttpGet("by-department/{departmentId:long}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<TeamDto>>>> GetByDepartment(long departmentId)
    {
        var result = await _teamService.GetTeamsByDepartmentIdAsync(departmentId);
        return HandleResult(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<SuccessResponse<PaginatedResponse<TeamGridItemDto>>>> GetPaged([FromQuery] TeamQueryParameters parameters)
    {
        var result = await _teamService.GetPagedAsync(parameters);
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<TeamDto>>>> GetAll()
    {
        var result = await _teamService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<TeamDto>>> GetById(long id)
    {
        var result = await _teamService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateTeamDto dto)
    {
        var result = await _teamService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateTeamDto dto)
    {
        var result = await _teamService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _teamService.DeleteAsync(id);
        return HandleResult(result);
    }
}
