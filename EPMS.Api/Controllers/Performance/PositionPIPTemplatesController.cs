using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PositionPIPTemplatesController : ApiControllerBase
{
    private readonly IPositionPIPTemplateService _positionPIPTemplateService;

    public PositionPIPTemplatesController(IPositionPIPTemplateService positionPIPTemplateService)
    {
        _positionPIPTemplateService = positionPIPTemplateService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>>> GetAll()
    {
        var result = await _positionPIPTemplateService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SuccessResponse<PositionPIPTemplateDto>>> GetById(long id)
    {
        var result = await _positionPIPTemplateService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("position/{positionId}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>>> GetByPositionId(long positionId)
    {
        var result = await _positionPIPTemplateService.GetByPositionIdAsync(positionId);
        return HandleResult(result);
    }

    [HttpGet("position/{positionId}/active")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PositionPIPTemplateDto>>>> GetActiveByPositionId(long positionId)
    {
        var result = await _positionPIPTemplateService.GetActiveByPositionIdAsync(positionId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreatePositionPIPTemplateDto dto)
    {
        var result = await _positionPIPTemplateService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdatePositionPIPTemplateDto dto)
    {
        var result = await _positionPIPTemplateService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _positionPIPTemplateService.DeleteAsync(id);
        return HandleResult(result);
    }
}