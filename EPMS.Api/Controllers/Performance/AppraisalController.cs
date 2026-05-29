using EPMS.Api.Authorization;
using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/performance/appraisals")]
[ApiController]
public class AppraisalController : ApiControllerBase
{
    private readonly IAppraisalService _service;
    private readonly IValidator<AppraisalSubmissionDto> _submitValidator;

    public AppraisalController(IAppraisalService service, IValidator<AppraisalSubmissionDto> submitValidator)
    {
        _service = service;
        _submitValidator = submitValidator;
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

    [Authorize]
    [HttpGet("{id:long}/view")]
    public async Task<ActionResult<SuccessResponse>> GetView(long id)
    {
        var result = await _service.GetAppraisalViewAsync(id);
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("my-kpi")]
    public async Task<ActionResult<SuccessResponse>> GetMyKpi()
    {
        var result = await _service.GetMyKpiAsync();
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin,SystemAdmin")]
    [HttpGet("pending")]
    public async Task<ActionResult<SuccessResponse>> GetPending()
    {
        var result = await _service.GetPendingAsync();
        return HandleResult(result);
    }

    [HttpGet("employee/{employeeId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByEmployeeId(long employeeId)
    {
        var result = await _service.GetByEmployeeIdAsync(employeeId);
        return HandleResult(result);
    }

    [HttpGet("my-evaluations")]
    public async Task<ActionResult<SuccessResponse>> GetMyEvaluations()
    {
        var result = await _service.GetMyEvaluationsAsync();
        return HandleResult(result);
    }

    [HttpGet("entity/{entityType}/cycle/{cycleId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByEntityTypeAndCycle(string entityType, long cycleId)
    {
        var result = await _service.GetByEntityTypeAndCycleAsync(entityType, cycleId);
        return HandleResult(result);
    }

    [HttpGet("entity/{entityType}")]
    public async Task<ActionResult<SuccessResponse>> GetByEntityType(string entityType)
    {
        var result = await _service.GetByEntityTypeAsync(entityType);
        return HandleResult(result);
    }

    [HttpPut("{id:long}/details")]
    public async Task<ActionResult<SuccessResponse>> UpdateDetailActualValues(long id, [FromBody] List<AppraisalDetailDto> details)
    {
        var result = await _service.UpdateDetailActualValuesAsync(id, details);
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
        await _submitValidator.ValidateAndThrowAsync(dto);
        var result = await _service.SubmitAsync(dto);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("{id:long}/finalize-kpi")]
    public async Task<ActionResult<SuccessResponse>> FinalizeKpi(long id)
    {
        var result = await _service.FinalizeKpiAsync(id);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("{id:long}/finalize-evaluation")]
    public async Task<ActionResult<SuccessResponse>> FinalizeEvaluation(long id, [FromQuery] string role)
    {
        var result = await _service.FinalizeEvaluationAsync(id, role);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPut("{id:long}/approve-self")]
    public async Task<ActionResult<SuccessResponse>> ApproveSelfAssessment(long id)
    {
        var result = await _service.ApproveSelfAssessmentAsync(id);
        return HandleResult(result);
    }

    [HttpGet("{id:long}/forms")]
    public async Task<ActionResult<SuccessResponse>> GetEmployeeForms(long id)
    {
        var result = await _service.GetEmployeeFormsOverviewAsync(id);
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("manager-self-pending")]
    public async Task<ActionResult<SuccessResponse>> GetManagerSelfPending()
    {
        var result = await _service.GetManagerSelfPendingAsync();
        return HandleResult(result);
    }

    [HttpPost("{id:long}/unlock-role")]
    public async Task<ActionResult<SuccessResponse>> UnlockRole(long id, [FromBody] UnlockRoleRequestDto dto)
    {
        var result = await _service.UnlockRoleAsync(id, dto.Role);
        return HandleResult(result);
    }
}

