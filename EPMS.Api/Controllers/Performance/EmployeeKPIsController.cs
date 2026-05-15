using EPMS.Api.Controllers.Common;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.EmployeeKPI;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/employee-kpis")]
    [ApiController]
    public class EmployeeKPIsController : ApiControllerBase
    {
        private readonly IEmployeeKPIService _employeeKPIService;

        public EmployeeKPIsController(IEmployeeKPIService employeeKPIService)
        {
            _employeeKPIService = employeeKPIService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeKPIDto>>>> GetAll()
        {
            var result = await _employeeKPIService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<EmployeeKPIDto>>> GetById(long id)
        {
            var result = await _employeeKPIService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpGet("by-employee/{employeeId:long}/cycle/{cycleId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeKPIDto>>>> GetByEmployeeAndCycle(long employeeId, long cycleId)
        {
            var result = await _employeeKPIService.GetByEmployeeAndCycleAsync(employeeId, cycleId);
            return HandleResult(result);
        }

        [HttpGet("by-cycle/{cycleId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<EmployeeKPIDto>>>> GetByCycle(long cycleId)
        {
            var result = await _employeeKPIService.GetByCycleAsync(cycleId);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateEmployeeKPIDto dto)
        {
            var result = await _employeeKPIService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateEmployeeKPIDto dto)
        {
            var result = await _employeeKPIService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _employeeKPIService.DeleteAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/restore")]
        public async Task<ActionResult<SuccessResponse>> Restore(long id)
        {
            var result = await _employeeKPIService.RestoreAsync(id);
            return HandleResult(result);
        }
    }
}