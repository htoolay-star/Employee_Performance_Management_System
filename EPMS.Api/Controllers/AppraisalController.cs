using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Form;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppraisalController : ControllerBase
    {
        private readonly IAppraisalService _appraisalService;

        // Constructor Injection - Scrutor will handle IAppraisalService registration automatically
        public AppraisalController(IAppraisalService appraisalService)
        {
            _appraisalService = appraisalService;
        }

        /// <summary>
        /// Submits an employee appraisal (Self, Manager, or 360)
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAppraisal(AppraisalSubmissionDto dto)
        {
            // Business Logic, Validations, and Condition checks are handled inside the service.
            // Any exceptions thrown (e.g., KeyNotFound, InvalidOperation) will be caught by 
            // the GlobalExceptionHandler middleware in Program.cs.

            var result = await _appraisalService.SubmitAppraisalAsync(dto);

            return Ok(result);
        }

        /// <summary>
        /// Gets detailed appraisal information by ID
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetAppraisalDetails(long id)
        {
            var result = await _appraisalService.GetAppraisalDetailsAsync(id);

            return Ok(result);
        }
    }
}