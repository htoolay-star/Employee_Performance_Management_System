using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/continuous-feedbacks")]
    [ApiController]
    public class ContinuousFeedbacksController : ApiControllerBase
    {
        private readonly IContinuousFeedbackService _feedbackService;

        public ContinuousFeedbacksController(IContinuousFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("received")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>>> GetReceived()
        {
            var result = await _feedbackService.GetReceivedFeedbackAsync();
            return HandleResult(result);
        }

        [HttpGet("given")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>>> GetGiven()
        {
            var result = await _feedbackService.GetGivenFeedbackAsync();
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>>> GetAll()
        {
            var result = await _feedbackService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<ContinuousFeedbackDto>>> GetById(long id)
        {
            var result = await _feedbackService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpGet("employee/{employeeId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>>> GetByEmployee(long employeeId)
        {
            var result = await _feedbackService.GetByEmployeeIdAsync(employeeId);
            return HandleResult(result);
        }

        [HttpGet("by-user/{userId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<ContinuousFeedbackDto>>>> GetByUser(long userId)
        {
            var result = await _feedbackService.GetByUserIdAsync(userId);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateContinuousFeedbackDto dto)
        {
            var result = await _feedbackService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateContinuousFeedbackDto dto)
        {
            var result = await _feedbackService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _feedbackService.DeleteAsync(id);
            return HandleResult(result);
        }
    }
}