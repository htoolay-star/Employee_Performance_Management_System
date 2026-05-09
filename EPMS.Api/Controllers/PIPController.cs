using EPMS.Api.Services.Interfaces;
using EPMS.Shared.DTOs.PerformanceImprovementPlan;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PIPController : ControllerBase
    {
        private readonly IPIPService _service;

        public PIPController(IPIPService service)
        {
            _service = service;
        }

        // Create PIP
        [HttpPost]
        public async Task<IActionResult> Create(CreatePIPDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        // Add Progress / Feedback
        [HttpPost("progress")]
        public async Task<IActionResult> AddProgress(AddPIPProgressDto dto)
        {
            await _service.AddProgressAsync(dto);
            return Ok("Progress added");
        }

        // Get All
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
