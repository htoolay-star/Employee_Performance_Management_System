using EPMS.Domain.Interfaces;
using EPMS.Shared.DTOs.HR;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers;

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
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DepartmentDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return Ok(new { Id = id, Message = "Created Successfully" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, DepartmentDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { Message = "Updated Successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _service.DeleteAsync(id);
        return Ok(new { Message = "Deleted Successfully" });
    }
}