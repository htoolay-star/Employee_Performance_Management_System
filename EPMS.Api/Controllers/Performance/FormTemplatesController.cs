using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/form-templates")]
    [ApiController]
    public class FormTemplatesController : ApiControllerBase
    {
        private readonly IFormTemplateService _formTemplateService;

        public FormTemplatesController(IFormTemplateService formTemplateService)
        {
            _formTemplateService = formTemplateService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<FormTemplateDto>>>> GetAll()
        {
            var result = await _formTemplateService.GetAllAsync();
            return HandleResult(result);
        }

    [HttpGet("active")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<FormTemplateDto>>>> GetActive()
    {
        var result = await _formTemplateService.GetActiveAsync();
        return HandleResult(result);
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<LookUpDto>>>> GetLookup()
    {
        var result = await _formTemplateService.GetLookupAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<FormTemplateDto>>> GetById(long id)
        {
            var result = await _formTemplateService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateFormTemplateDto dto)
        {
            var result = await _formTemplateService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateFormTemplateDto dto)
        {
            var result = await _formTemplateService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _formTemplateService.DeleteAsync(id);
        return HandleResult(result);
    }

    [HttpGet("{id:long}/preview")]
    public async Task<ActionResult<SuccessResponse<FormTemplatePreviewDto>>> GetPreview(long id)
    {
        var result = await _formTemplateService.GetPreviewAsync(id);
        return HandleResult(result);
    }
}
}