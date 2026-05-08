using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PositionFormTemplateDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PositionFormTemplatesController : ApiControllerBase
{
    private readonly IPositionFormTemplateService _positionFormTemplateService;

    public PositionFormTemplatesController(IPositionFormTemplateService positionFormTemplateService)
    {
        _positionFormTemplateService = positionFormTemplateService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PositionFormTemplateDto>>>> GetAll()
    {
        var result = await _positionFormTemplateService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SuccessResponse<PositionFormTemplateDto>>> GetById(long id)
    {
        var result = await _positionFormTemplateService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("position/{positionId}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PositionFormTemplateDto>>>> GetByPositionId(long positionId)
    {
        var result = await _positionFormTemplateService.GetByPositionIdAsync(positionId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreatePositionFormTemplateDto dto)
    {
        var result = await _positionFormTemplateService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdatePositionFormTemplateDto dto)
    {
        var result = await _positionFormTemplateService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _positionFormTemplateService.DeleteAsync(id);
        return HandleResult(result);
    }
}