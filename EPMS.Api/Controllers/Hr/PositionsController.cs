using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.PositionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly IPositionService _service;

    public PositionsController(IPositionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePositionDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return Ok(new { Id = id, Message = "Created Successfully" });
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdatePositionDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { Message = "Updated Successfully" });
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
