using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/performance/kpi-weight-priorities")]
[ApiController]
public class KPIWeightPrioritiesController : ApiControllerBase
{
    private readonly IKPIWeightPriorityService _kpiWeightPriorityService;

    public KPIWeightPrioritiesController(IKPIWeightPriorityService kpiWeightPriorityService)
    {
        _kpiWeightPriorityService = kpiWeightPriorityService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<KPIWeightPriorityDto>>>> GetAll()
    {
        var result = await _kpiWeightPriorityService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<KPIWeightPriorityDto>>>> GetActive()
    {
        var result = await _kpiWeightPriorityService.GetActiveAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<KPIWeightPriorityDto>>> GetById(long id)
    {
        var result = await _kpiWeightPriorityService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-level/{levelName}")]
    public async Task<ActionResult<SuccessResponse<KPIWeightPriorityDto>>> GetByLevelName(string levelName)
    {
        var result = await _kpiWeightPriorityService.GetByLevelNameAsync(levelName);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateKPIWeightPriorityDto dto)
    {
        var result = await _kpiWeightPriorityService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateKPIWeightPriorityDto dto)
    {
        var result = await _kpiWeightPriorityService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _kpiWeightPriorityService.DeleteAsync(id);
        return HandleResult(result);
    }

    [HttpPost("{id:long}/deactivate")]
    public async Task<ActionResult<SuccessResponse>> Deactivate(long id)
    {
        var result = await _kpiWeightPriorityService.DeactivateAsync(id);
        return HandleResult(result);
    }

    [HttpPost("{id:long}/reactivate")]
    public async Task<ActionResult<SuccessResponse>> Reactivate(long id)
    {
        var result = await _kpiWeightPriorityService.ReactivateAsync(id);
        return HandleResult(result);
    }
}
