using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PIPObjectivesController : ApiControllerBase
{
    private readonly IPIPObjectiveService _pipObjectiveService;

    public PIPObjectivesController(IPIPObjectiveService pipObjectiveService)
    {
        _pipObjectiveService = pipObjectiveService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PIPObjectiveDto>>>> GetAll()
    {
        var result = await _pipObjectiveService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SuccessResponse<PIPObjectiveDto>>> GetById(long id)
    {
        var result = await _pipObjectiveService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("pip/{pipId}")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<PIPObjectiveDto>>>> GetByPIPId(long pipId)
    {
        var result = await _pipObjectiveService.GetByPIPIdAsync(pipId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreatePIPObjectiveDto dto)
    {
        var result = await _pipObjectiveService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdatePIPObjectiveDto dto)
    {
        var result = await _pipObjectiveService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _pipObjectiveService.DeleteAsync(id);
        return HandleResult(result);
    }
}