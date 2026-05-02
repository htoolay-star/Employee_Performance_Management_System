using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.LevelDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
public class LevelsController : ControllerBase
{
    private readonly ILevelService _service;

    public LevelsController(ILevelService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLevelDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return Ok(new { Id = id, Message = "Created Successfully" });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateLevelDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { Message = "Updated Successfully" });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
