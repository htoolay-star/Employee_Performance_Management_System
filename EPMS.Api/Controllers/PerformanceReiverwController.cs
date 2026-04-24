using Microsoft.AspNetCore.Mvc;
using EPMS.Api.Services.Interfaces;
using EPMS.Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
public class PerformanceReviewController : ControllerBase
{
    private readonly IPerformanceReviewService _service;

    public PerformanceReviewController(IPerformanceReviewService service)
    {
        _service = service;
    }

    // Create
    [HttpPost]
    public async Task<IActionResult> Create(CreatePerformanceReivewDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    // Get All
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // Get By Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}