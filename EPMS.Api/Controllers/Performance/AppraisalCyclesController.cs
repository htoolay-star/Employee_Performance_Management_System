using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/appraisal-cycles")]
    [ApiController]
    public class AppraisalCyclesController : ApiControllerBase
    {
        private readonly IAppraisalCycleService _appraisalCycleService;

        public AppraisalCyclesController(IAppraisalCycleService appraisalCycleService)
        {
            _appraisalCycleService = appraisalCycleService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<AppraisalCycleDto>>>> GetAll()
        {
            var result = await _appraisalCycleService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("active")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<AppraisalCycleDto>>>> GetActive()
        {
            var result = await _appraisalCycleService.GetActiveCyclesAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<AppraisalCycleDto>>> GetById(long id)
        {
            var result = await _appraisalCycleService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateAppraisalCycleDto dto)
        {
            var result = await _appraisalCycleService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateAppraisalCycleDto dto)
        {
            var result = await _appraisalCycleService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _appraisalCycleService.DeleteAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/lock")]
        public async Task<ActionResult<SuccessResponse>> Lock(long id)
        {
            var result = await _appraisalCycleService.LockCycleAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/deactivate")]
        public async Task<ActionResult<SuccessResponse>> Deactivate(long id)
        {
            var result = await _appraisalCycleService.DeactivateAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/reactivate")]
        public async Task<ActionResult<SuccessResponse>> Reactivate(long id)
        {
            var result = await _appraisalCycleService.ReactivateAsync(id);
            return HandleResult(result);
        }
    }
}