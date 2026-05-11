using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.Features.Positions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ApiControllerBase
{
    private readonly IPositionService _service;

    public PositionsController(IPositionService service)
    {
        _service = service;
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<LookUpDto>>>> GetLookup()
    {
        var result = await _service.GetLookupAsync();
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PositionDto>>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<SuccessResponse<PaginatedResponse<PositionGridItemDto>>>> GetPaged([FromQuery] PositionQueryParameters parameters)
    {
        var result = await _service.GetPagedAsync(parameters);
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<PositionDto>>> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreatePositionDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdatePositionDto dto)
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

    // Permission management endpoints
    [HttpGet("{positionId:long}/permissions")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PermissionDto>>>> GetPermissions(long positionId)
    {
        var result = await _service.GetPermissionsForPositionAsync(positionId);
        return HandleResult(result);
    }

    [HttpPost("{positionId:long}/permissions/{permissionId:long}")]
    public async Task<ActionResult<SuccessResponse>> AssignPermission(long positionId, long permissionId)
    {
        var result = await _service.AssignPermissionToPositionAsync(positionId, permissionId);
        return HandleResult(result);
    }

    [HttpDelete("{positionId:long}/permissions/{permissionId:long}")]
    public async Task<ActionResult<SuccessResponse>> RemovePermission(long positionId, long permissionId)
    {
        var result = await _service.RemovePermissionFromPositionAsync(positionId, permissionId);
        return HandleResult(result);
    }
}
