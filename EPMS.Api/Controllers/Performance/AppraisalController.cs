using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Form;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPMS.Api.Controllers.Performance
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppraisalController : ControllerBase
    {
        private readonly IAppraisalService _appraisalService;

        public AppraisalController(IAppraisalService appraisalService)
        {
            _appraisalService = appraisalService;
        }

        /// <summary>
        /// Submits performance assessment results and calculates the final grade.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] AppraisalSubmissionDto dto)
        {
            // Note: Validation is handled by FluentValidation before hitting this point.
            // Note: Exception/Error handling is handled by Global Exception Middleware.

            var result = await _appraisalService.SubmitAppraisalAsync(dto);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves detailed appraisal scores by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _appraisalService.GetAppraisalDetailsAsync(id);
            return Ok(result);
        }
    }
}