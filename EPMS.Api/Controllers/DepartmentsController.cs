using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Shared.DTOs.HR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentsController(AppDbContext context)
    {
        _context = context;
    }

 
    [HttpPost]
    public async Task<IActionResult> Create(DepartmentDto dto)
    {
        var department = new Department(dto.Name, dto.Code);
        
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return Ok(new { Id = department.Id, Message = "Department created successfully!" });
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
    {
        var departments = await _context.Departments
            .Select(d => new DepartmentDto
            { 
                Id = d.Id, 
                Name = d.Name, 
                Code = d.Code 
            })
            .ToListAsync();

        return Ok(departments);
    }
}