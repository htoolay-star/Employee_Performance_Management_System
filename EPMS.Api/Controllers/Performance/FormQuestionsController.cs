using EPMS.Api.Controllers.Common;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/performance/form-questions")]
[ApiController]
public class FormQuestionsController : ApiControllerBase
{
    private readonly IFormQuestionService _service;

    public FormQuestionsController(IFormQuestionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse>> Create([FromBody] CreateFormQuestionDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateFormQuestionDto dto)
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

    [HttpGet("template/{templateId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByTemplateId(long templateId)
    {
        var result = await _service.GetByTemplateIdAsync(templateId);
        return HandleResult(result);
    }

    [HttpGet("category/{categoryId:long}")]
    public async Task<ActionResult<SuccessResponse>> GetByCategoryId(long categoryId)
    {
        var result = await _service.GetByCategoryIdAsync(categoryId);
        return HandleResult(result);
    }
}