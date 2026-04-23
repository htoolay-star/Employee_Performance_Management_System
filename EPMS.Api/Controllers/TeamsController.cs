using EPMS.Shared.DTOs.HR;
using Microsoft.AspNetCore.Mvc;
using EPMS.Domain.Interfaces;

namespace EPMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _teamService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _teamService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTeamDto dto)
    {
        try
        {
            var id = await _teamService.CreateAsync(dto);
            return Ok(new { Id = id, Message = "Created Successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, TeamDto dto)
    {
        try
        {
            await _teamService.UpdateAsync(id, dto);
            return Ok(new { Message = "Updated Successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _teamService.DeleteAsync(id);
        return Ok(new { Message = "Deleted Successfully" });
    }
}
