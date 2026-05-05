using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interfaces;
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

    [HttpGet]
    public async Task<ActionResult<SuccessResponse<IEnumerable<LevelDto>>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SuccessResponse<LevelDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessResponse<int>>> Create(CreateLevelDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SuccessResponse>> Update(int id, UpdateLevelDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<SuccessResponse>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return HandleResult(result);
    }
}
