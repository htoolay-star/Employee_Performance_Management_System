using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuestionRatingScalesController : ApiControllerBase
{
    private readonly IQuestionRatingScaleService _questionRatingScaleService;

    public QuestionRatingScalesController(IQuestionRatingScaleService questionRatingScaleService)
    {
        _questionRatingScaleService = questionRatingScaleService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>>> GetAll()
    {
        var result = await _questionRatingScaleService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<QuestionRatingScaleDto>>>> GetActive()
    {
        var result = await _questionRatingScaleService.GetActiveAsync();
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SuccessResponse<QuestionRatingScaleDto>>> GetById(long id)
    {
        var result = await _questionRatingScaleService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateQuestionRatingScaleDto dto)
    {
        var result = await _questionRatingScaleService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateQuestionRatingScaleDto dto)
    {
        var result = await _questionRatingScaleService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _questionRatingScaleService.DeleteAsync(id);
        return HandleResult(result);
    }

    [HttpPut("{id}/deactivate")]
    public async Task<ActionResult<SuccessResponse>> Deactivate(long id)
    {
        var result = await _questionRatingScaleService.DeactivateAsync(id);
        return HandleResult(result);
    }

    [HttpPut("{id}/reactivate")]
    public async Task<ActionResult<SuccessResponse>> Reactivate(long id)
    {
        var result = await _questionRatingScaleService.ReactivateAsync(id);
        return HandleResult(result);
    }
}