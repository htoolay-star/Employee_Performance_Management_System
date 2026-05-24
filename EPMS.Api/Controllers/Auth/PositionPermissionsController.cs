using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Auth;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;
using EPMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Auth;

[ApiController]
[Route("api/position-permissions")]
[Authorize(Roles = RoleConstants.SA_Admin)]
public class PositionPermissionsController : ApiControllerBase
{
    private readonly IPositionPermissionService _service;

    public PositionPermissionsController(IPositionPermissionService service)
    {
        _service = service;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("position/{positionId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByPositionId(long positionId)
    {
        var result = await _service.GetByPositionIdAsync(positionId);
        return HandleResult(result);
    }

    [HttpGet("permission/{permissionId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByPermissionId(long permissionId)
    {
        var result = await _service.GetByPermissionIdAsync(permissionId);
        return HandleResult(result);
    }

    [HttpGet("position/{positionId:long}/permissions")]
    public async Task<ActionResult<SuccessResponse>> GetPermissionsForPosition(long positionId)
    {
        var result = await _service.GetPermissionsForPositionAsync(positionId);
        return HandleResult(result);
    }

    [HttpPut("position/{positionId:long}")]
    public async Task<ActionResult<SuccessResponse>> UpdatePositionPermissions(
        long positionId, [FromBody] List<long> permissionIds)
    {
        var result = await _service.UpdatePositionPermissionsAsync(positionId, permissionIds);
        return HandleResult(result);
    }
}
