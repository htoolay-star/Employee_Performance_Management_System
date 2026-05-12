using EPMS.Api.Services.Interfaces;
using EPMS.Shared.DTOs.KPI;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeKPIController : ControllerBase
    {
        private readonly IEmployeeKPIService _service;

        public EmployeeKPIController(
            IEmployeeKPIService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(
            SubmitEmployeeKPIDto dto)
        {
            await _service.SubmitAsync(dto);

            return Ok();
        }

        [HttpGet("{employeeId}/{cycleId}")]
        public async Task<IActionResult> GetResults(
            long employeeId,
            long cycleId)
        {
            var result =
                await _service.GetEmployeeResultsAsync(
                    employeeId,
                    cycleId);

            return Ok(result);
        }
    }
}
