using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.DepartmentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Hr;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentsController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateDepartmentDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, new { id, message = "Created successfully." });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdateDepartmentDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { Message = "Updated Successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}