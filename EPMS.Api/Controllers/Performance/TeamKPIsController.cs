using EPMS.Api.Controllers.Common;
using EPMS.Domain.Services.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.Performance.TeamKPI;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/team-kpis")]
    [ApiController]
    public class TeamKPIsController : ApiControllerBase
    {
        private readonly ITeamKPIService _teamKPIService;

        public TeamKPIsController(ITeamKPIService teamKPIService)
        {
            _teamKPIService = teamKPIService;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<TeamKPIDto>>>> GetAll()
        {
            var result = await _teamKPIService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<TeamKPIDto>>> GetById(long id)
        {
            var result = await _teamKPIService.GetByIdAsync(id);
            return HandleResult(result);
        }

        [HttpGet("team/{teamId:long}")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<TeamKPIDto>>>> GetByTeamId(long teamId)
        {
            var result = await _teamKPIService.GetByTeamIdAsync(teamId);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<long>>> Create([FromBody] CreateTeamKPIDto dto)
        {
            var result = await _teamKPIService.CreateAsync(dto);
            return HandleResult(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Update(long id, [FromBody] UpdateTeamKPIDto dto)
        {
            var result = await _teamKPIService.UpdateAsync(id, dto);
            return HandleResult(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponse>> Delete(long id)
        {
            var result = await _teamKPIService.DeleteAsync(id);
            return HandleResult(result);
        }
    }
}
