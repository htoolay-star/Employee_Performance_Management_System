using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/kpi-masters")]
    [ApiController]
    public class KPIMastersController : ApiControllerBase
    {
        private readonly IKPIMasterService _kpiMasterService;

        public KPIMastersController(IKPIMasterService kpiMasterService)
        {
            _kpiMasterService = kpiMasterService;
        }

        [HttpGet("lookup")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<LookUpDto>>>> GetLookup()
        {
            var result = await _kpiMasterService.GetLookupAsync();
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<KPIMasterDto>>>> GetAll()
        {
            var result = await _kpiMasterService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("active")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<KPIMasterDto>>>> GetActive()
        {
            var result = await _kpiMasterService.GetActiveAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<KPIMasterDto>>> GetById(long id)
        {
            var result = await _kpiMasterService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateKPIMasterDto dto)
        {
            var result = await _kpiMasterService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateKPIMasterDto dto)
        {
            var result = await _kpiMasterService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _kpiMasterService.DeleteAsync(id);
            return HandleResult(result);
        }
    }
}