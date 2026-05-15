using EPMS.Api.Controllers.Common;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.EntityKPI;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/entity-kpis")]
    [ApiController]
    public class EntityKPIsController : ApiControllerBase
    {
        private readonly IEntityKPIService _entityKPIService;

        public EntityKPIsController(IEntityKPIService entityKPIService)
        {
            _entityKPIService = entityKPIService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<EntityKPIDto>>>> GetAll()
        {
            var result = await _entityKPIService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<EntityKPIDto>>> GetById(long id)
        {
            var result = await _entityKPIService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpGet("by-entity")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<EntityKPIDto>>>> GetByEntity([FromQuery] string entityType, [FromQuery] long entityId)
        {
            var result = await _entityKPIService.GetByEntityAsync(entityType, entityId);
            return HandleResult(result);
        }

        [HttpGet("by-type/{entityType}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<EntityKPIDto>>>> GetByEntityType(string entityType)
        {
            var result = await _entityKPIService.GetByEntityTypeAsync(entityType);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateEntityKPIDto dto)
        {
            var result = await _entityKPIService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateEntityKPIDto dto)
        {
            var result = await _entityKPIService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _entityKPIService.DeleteAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id:long}/restore")]
        public async Task<ActionResult<SuccessResponse>> Restore(long id)
        {
            var result = await _entityKPIService.RestoreAsync(id);
            return HandleResult(result);
        }
    }
}