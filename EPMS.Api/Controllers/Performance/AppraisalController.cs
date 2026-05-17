using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/performance/appraisals")]
[ApiController]
public class AppraisalController : ApiControllerBase
{
    private readonly IAppraisalService _service;

    public AppraisalController(IAppraisalService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("{id:long}/fill")]
    public async Task<ActionResult<SuccessResponse>> GetFill(long id)
    {
        var result = await _service.GetAppraisalFillAsync(id);
        return HandleResult(result);
    }

    [HttpGet("employee/{employeeId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByEmployeeId(long employeeId)
    {
        var result = await _service.GetByEmployeeIdAsync(employeeId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse>> Create([FromBody] CreateAppraisalDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateAppraisalDto dto)
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

    [HttpPost("submit")]
    public async Task<ActionResult<SuccessResponse>> Submit([FromBody] AppraisalSubmissionDto dto)
    {
        var result = await _service.SubmitAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}/lock")]
    public async Task<ActionResult<SuccessResponse>> Lock(long id, [FromBody] UnlockRequestDto request)
    {
        var result = await _service.LockAsync(id, request.AdminId, request.Reason);
        return HandleResult(result);
    }

    [HttpPut("{id:long}/unlock")]
    public async Task<ActionResult<SuccessResponse>> Unlock(long id, [FromBody] UnlockRequestDto request)
    {
        var result = await _service.UnlockAsync(id, request.AdminId, request.Reason);
        return HandleResult(result);
    }
}

