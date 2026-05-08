using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/performance/appraisals/recommendations")]
[ApiController]
public class AppraisalRecommendationsController : ApiControllerBase
{
    private readonly IAppraisalRecommendationService _service;

    public AppraisalRecommendationsController(IAppraisalRecommendationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse>> Create([FromBody] CreateAppraisalRecommendationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateAppraisalRecommendationDto dto)
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

    [HttpPut("{id:long}/approve")]
    public async Task<ActionResult<SuccessResponse>> Approve(long id, [FromBody] ProcessRecommendationDto request)
    {
        var result = await _service.ApproveAsync(id, request.HrAdminId, request.Comments);
        return HandleResult(result);
    }

    [HttpPut("{id:long}/reject")]
    public async Task<ActionResult<SuccessResponse>> Reject(long id, [FromBody] ProcessRecommendationDto request)
    {
        var result = await _service.RejectAsync(id, request.HrAdminId, request.Reason);
        return HandleResult(result);
    }
}

public record ProcessRecommendationDto(long HrAdminId, string? Comments = null, string? Reason = null);
