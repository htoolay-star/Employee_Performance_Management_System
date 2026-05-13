using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Hr;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.LevelDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
public class LevelsController : ApiControllerBase
{
    private readonly ILevelService _service;

    public LevelsController(ILevelService service)
    {
        _service = service;
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<SuccessResponse<IEnumerable<LookUpDto>>>> GetLookup()
    {
        var result = await _service.GetLookupAsync();
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<LevelDto>>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SuccessResponse<LevelDto>>> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<long>>> Create(CreateLevelDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SuccessResponse>> Update(long id, UpdateLevelDto dto)
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
}
