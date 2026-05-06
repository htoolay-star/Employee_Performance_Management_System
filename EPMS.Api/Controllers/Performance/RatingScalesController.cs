using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance;

[Route("api/performance/rating-scales")]
[ApiController]
[Authorize(Roles = RoleConstants.Admin)]
public class RatingScalesController : ApiControllerBase
{
    private readonly IRatingScaleService _ratingScaleService;

    public RatingScalesController(IRatingScaleService ratingScaleService)
    {
        _ratingScaleService = ratingScaleService;
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<RatingScaleDto>>>> GetAll()
    {
        var result = await _ratingScaleService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<RatingScaleDto>>>> GetActive()
    {
        var result = await _ratingScaleService.GetActiveAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<RatingScaleDto>>> GetById(long id)
    {
        var result = await _ratingScaleService.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("by-rating/{rating:int}")]
    public async Task<ActionResult<SuccessResponse<RatingScaleDto>>> GetByRating(int rating)
    {
        var result = await _ratingScaleService.GetByRatingAsync(rating);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateRatingScaleDto dto)
    {
        var result = await _ratingScaleService.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateRatingScaleDto dto)
    {
        var result = await _ratingScaleService.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Delete(long id)
    {
        var result = await _ratingScaleService.DeleteAsync(id);
        return HandleResult(result);
    }

    [HttpPost("{id:long}/deactivate")]
    public async Task<ActionResult<SuccessResponse>> Deactivate(long id)
    {
        var result = await _ratingScaleService.DeactivateAsync(id);
        return HandleResult(result);
    }

    [HttpPost("{id:long}/reactivate")]
    public async Task<ActionResult<SuccessResponse>> Reactivate(long id)
    {
        var result = await _ratingScaleService.ReactivateAsync(id);
        return HandleResult(result);
    }
}
