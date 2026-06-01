using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/pips")]
    [ApiController]
    public class PIPsController : ApiControllerBase
    {
        private readonly IPIPService _pipService;

        public PIPsController(IPIPService pipService)
        {
            _pipService = pipService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PIPDto>>>> GetAll()
        {
            var result = await _pipService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("my")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PIPDto>>>> GetMyPIPs()
        {
            var result = await _pipService.GetMyPIPsAsync();
            return HandleResult(result);
        }

        [HttpGet("active")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PIPDto>>>> GetActive()
        {
            var result = await _pipService.GetActivePIPsAsync();
            return HandleResult(result);
        }

        [HttpGet("employee/{employeeId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PIPDto>>>> GetByEmployee(long employeeId)
        {
            var result = await _pipService.GetByEmployeeIdAsync(employeeId);
            return HandleResult(result);
        }

        [HttpGet("manager/{managerId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PIPDto>>>> GetByManager(long managerId)
        {
            var result = await _pipService.GetByManagerIdAsync(managerId);
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<PIPDto>>> GetById(long id)
        {
            var result = await _pipService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreatePIPDto dto)
        {
            var result = await _pipService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdatePIPDto dto)
        {
            var result = await _pipService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _pipService.DeleteAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/conclude")]
        public async Task<ActionResult<SuccessResponse>> Conclude(long id, [FromBody] ConcludePIPDto dto)
        {
            var result = await _pipService.ConcludeAsync(id, dto.IsSuccessful, dto.Notes);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/extend")]
        public async Task<ActionResult<SuccessResponse>> Extend(long id, [FromBody] ExtendPIPDto dto)
        {
            var result = await _pipService.ExtendAsync(id, dto.NewEndDate, dto.Reason);
            return HandleResult(result);
        }
    }

    public class ConcludePIPDto
    {
        public bool IsSuccessful { get; set; }
        public string? Notes { get; set; }
    }

    public class ExtendPIPDto
    {
        public DateOnly NewEndDate { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}