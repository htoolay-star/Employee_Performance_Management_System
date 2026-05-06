using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interfaces;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.DTOs.TeamDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = RoleConstants.Admin)]
public class DepartmentsController : ApiControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentsController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<DepartmentDto>>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<DepartmentDto>>> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateDepartmentDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateDepartmentDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _service.DeleteAsync(id);
        return HandleResult(result);
    }

    // Team management endpoints
    [HttpGet("{departmentId:long}/teams")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<TeamDto>>>> GetTeams(long departmentId)
    {
        var result = await _service.GetTeamsForDepartmentAsync(departmentId);
        return HandleResult(result);
    }

    [HttpPost("{departmentId:long}/teams")]
    public async Task<ActionResult<SuccessResponse>> AddTeam(long departmentId, [FromBody] string teamName)
    {
        var result = await _service.AddTeamToDepartmentAsync(departmentId, teamName);
        return HandleResult(result);
    }

    [HttpDelete("{departmentId:long}/teams/{teamId:long}")]
    public async Task<ActionResult<SuccessResponse>> RemoveTeam(long departmentId, long teamId)
    {
        var result = await _service.RemoveTeamFromDepartmentAsync(departmentId, teamId);
        return HandleResult(result);
    }
}