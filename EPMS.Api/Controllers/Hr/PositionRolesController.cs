using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PositionRolesController : ApiControllerBase
{
    private readonly IPositionRoleService _service;

    public PositionRolesController(IPositionRoleService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all roles assigned to a position
    /// </summary>
    [HttpGet("positions/{positionId:long}/roles")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PositionRoleDto>>>> GetRolesForPosition(long positionId)
    {
        var result = await _service.GetRolesForPositionAsync(positionId);
        return HandleResult(result);
    }

    /// <summary>
    /// Assign a role to a position
    /// </summary>
    [HttpPost("positions/{positionId:long}/roles/{roleId:long}")]
    [Authorize(Roles = "SystemAdmin,Admin")]
    public async Task<ActionResult<SuccessResponse<long>>> AssignRoleToPosition(long positionId, long roleId)
    {
        var result = await _service.AssignRoleToPositionAsync(positionId, roleId);
        return HandleResult(result);
    }

    /// <summary>
    /// Remove a role from a position
    /// </summary>
    [HttpDelete("positions/{positionId:long}/roles/{roleId:long}")]
    [Authorize(Roles = "SystemAdmin,Admin")]
    public async Task<ActionResult<SuccessResponse>> RemoveRoleFromPosition(long positionId, long roleId)
    {
        var result = await _service.RemoveRoleFromPositionAsync(positionId, roleId);
        return HandleResult(result);
    }

    /// <summary>
    /// Get all positions that have a specific role
    /// </summary>
    [HttpGet("roles/{roleId:long}/positions")]
    [Authorize(Roles = "SystemAdmin,Admin")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PositionRoleDto>>>> GetPositionsForRole(long roleId)
    {
        var result = await _service.GetPositionsForRoleAsync(roleId);
        return HandleResult(result);
    }
}