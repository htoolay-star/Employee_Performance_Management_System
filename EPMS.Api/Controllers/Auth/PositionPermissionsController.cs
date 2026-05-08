using EPMS.Api.Controllers.Common;
using EPMS.Domain.Services.Auth;
using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;
using EPMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Auth;

[ApiController]
[Route("api/position-permissions")]
public class PositionPermissionsController : ApiControllerBase
{
    private readonly IPositionPermissionService _service;

    public PositionPermissionsController(IPositionPermissionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse>> Create(CreatePositionPermissionDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpDelete("position/{positionId:long}/permission/{permissionId:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long positionId, long permissionId)
    {
        var result = await _service.DeleteAsync(positionId, permissionId);
        return HandleResult(result);
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
}
