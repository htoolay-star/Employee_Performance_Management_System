using EPMS.Api.Controllers.Common;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.DeptKPI;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/dept-kpis")]
    [ApiController]
    public class DeptKPIsController : ApiControllerBase
    {
        private readonly IDeptKPIService _deptKPIService;

        public DeptKPIsController(IDeptKPIService deptKPIService)
        {
            _deptKPIService = deptKPIService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<DeptKPIDto>>>> GetAll()
        {
            var result = await _deptKPIService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<DeptKPIDto>>> GetById(long id)
        {
            var result = await _deptKPIService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpGet("department/{deptId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<DeptKPIDto>>>> GetByDeptId(long deptId)
        {
            var result = await _deptKPIService.GetByDeptIdAsync(deptId);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateDeptKPIDto dto)
        {
            var result = await _deptKPIService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateDeptKPIDto dto)
        {
            var result = await _deptKPIService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _deptKPIService.DeleteAsync(id);
            return HandleResult(result);
        }
    }
}
