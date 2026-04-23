using EPMS.Shared.DTOs.HR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _teamService.GetAllTeamsAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTeamDto dto)
    {
        var result = await _teamService.CreateTeamAsync(dto);
        return Ok(result);
    }
}