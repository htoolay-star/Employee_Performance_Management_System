using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/performance/evaluation-responses")]
[ApiController]
public class EvaluationResponsesController : ApiControllerBase
{
    private readonly IEvaluationResponseService _service;

    public EvaluationResponsesController(IEvaluationResponseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse>> Create([FromBody] CreateEvaluationResponseDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateEvaluationResponseDto dto)
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

    [HttpGet("appraisal/{appraisalId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByAppraisalId(long appraisalId)
    {
        var result = await _service.GetByAppraisalIdAsync(appraisalId);
        return HandleResult(result);
    }

    [HttpGet("template/{templateId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByTemplateId(long templateId)
    {
        var result = await _service.GetByTemplateIdAsync(templateId);
        return HandleResult(result);
    }

    [HttpGet("question/{questionId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByQuestionId(long questionId)
    {
        var result = await _service.GetByQuestionIdAsync(questionId);
        return HandleResult(result);
    }

    [HttpGet("appraisal/{appraisalId:long}/my-responses")]
    public async Task<ActionResult<SuccessResponse>> GetFormFill(
        long appraisalId,
        [FromQuery] string role)
    {
        var result = await _service.GetFormFillAsync(appraisalId, role);
        return HandleResult(result);
    }

    [HttpGet("appraisal/{appraisalId:long}/self-assessment")]
    public async Task<ActionResult<SuccessResponse>> GetSelfAssessment(
        long appraisalId)
    {
        var result = await _service.GetSelfAssessmentAsync(appraisalId);
        return HandleResult(result);
    }

    [HttpGet("appraisal/{appraisalId:long}/view")]
    public async Task<ActionResult<SuccessResponse>> GetEvaluationView(
        long appraisalId,
        [FromQuery] string role,
        [FromQuery] long? evaluatorId = null)
    {
        var result = await _service.GetEvaluationViewAsync(appraisalId, role, evaluatorId);
        return HandleResult(result);
    }

    [HttpPost("submit-role")]
    public async Task<ActionResult<SuccessResponse>> SubmitRoleResponses([FromBody] SubmitRoleRequestDto dto)
    {
        var result = await _service.SubmitRoleResponsesAsync(dto.AppraisalId, dto.Role);
        return HandleResult(result);
    }

    [HttpGet("my-forms")]
    public async Task<ActionResult<SuccessResponse>> GetMyForms([FromQuery] string? roleGroup = null)
    {
        var result = await _service.GetMyFormsAsync(roleGroup);
        return HandleResult(result);
    }

    [HttpGet("my-appraisal-forms")]
    public async Task<ActionResult<SuccessResponse>> GetMyAppraisalForms()
    {
        var result = await _service.GetMyAppraisalFormsAsync();
        return HandleResult(result);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<SuccessResponse>> GetPendingEvaluations()
    {
        var result = await _service.GetPendingEvaluationsAsync();
        return HandleResult(result);
    }
}