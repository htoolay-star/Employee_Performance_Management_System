using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/one-on-one-meetings")]
    [ApiController]
    public class OneOnOneMeetingsController : ApiControllerBase
    {
        private readonly IOneOnOneMeetingService _meetingService;

        public OneOnOneMeetingsController(IOneOnOneMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>>> GetAll()
        {
            var result = await _meetingService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>>> GetUpcoming()
        {
            var result = await _meetingService.GetUpcomingAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<OneOnOneMeetingDto>>> GetById(long id)
        {
            var result = await _meetingService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpGet("employee/{employeeId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>>> GetByEmployee(long employeeId)
        {
            var result = await _meetingService.GetByEmployeeIdAsync(employeeId);
            return HandleResult(result);
        }

        [HttpGet("manager/{managerId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<OneOnOneMeetingDto>>>> GetByManager(long managerId)
        {
            var result = await _meetingService.GetByManagerIdAsync(managerId);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateOneOnOneMeetingDto dto)
        {
            var result = await _meetingService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateOneOnOneMeetingDto dto)
        {
            var result = await _meetingService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _meetingService.DeleteAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/complete")]
        public async Task<ActionResult<SuccessResponse>> Complete(long id, [FromBody] CompleteMeetingDto dto)
        {
            var result = await _meetingService.CompleteAsync(id, dto);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/cancel")]
        public async Task<ActionResult<SuccessResponse>> Cancel(long id)
        {
            var result = await _meetingService.CancelAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/acknowledge")]
        public async Task<ActionResult<SuccessResponse>> Acknowledge(long id)
        {
            var result = await _meetingService.AcknowledgeAsync(id);
            return HandleResult(result);
        }
    }
}