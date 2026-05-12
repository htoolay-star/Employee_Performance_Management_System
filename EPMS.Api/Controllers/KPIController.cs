using EPMS.Api.Services.Interfaces;
using EPMS.Shared.DTOs.KPI;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KPIController : ControllerBase
    {
        private readonly IKPIService _service;

        public KPIController(IKPIService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateKPIMasterDto dto)
        {
            await _service.CreateAsync(dto);

            return Ok();
        }
    }
}
