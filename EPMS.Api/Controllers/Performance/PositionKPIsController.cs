using EPMS.Api.Controllers.Common;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.PositionKPI;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/position-kpis")]
    [ApiController]
    public class PositionKPIsController : ApiControllerBase
    {
        private readonly IPositionKPIService _positionKPIService;

        public PositionKPIsController(IPositionKPIService positionKPIService)
        {
            _positionKPIService = positionKPIService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PositionKPIDto>>>> GetAll()
        {
            var result = await _positionKPIService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<PositionKPIDto>>> GetById(long id)
        {
            var result = await _positionKPIService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpGet("position/{positionId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PositionKPIDto>>>> GetByPositionId(long positionId)
        {
            var result = await _positionKPIService.GetByPositionIdAsync(positionId);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreatePositionKPIDto dto)
        {
            var result = await _positionKPIService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdatePositionKPIDto dto)
        {
            var result = await _positionKPIService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _positionKPIService.DeleteAsync(id);
            return HandleResult(result);
        }
    }
}