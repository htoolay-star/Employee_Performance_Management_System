using EPMS.Api.Controllers.Common;
using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.Api.Controllers.Performance
{
    [Route("api/performance/appraisals")]
    [ApiController]
    public class AppraisalController : ApiControllerBase
    {
        private readonly IAppraisalService _appraisalService;

        public AppraisalController(IAppraisalService appraisalService)
        {
            _appraisalService = appraisalService;
        }

        [HttpPost("submit")]
        public async Task<ActionResult<SuccessResponse<AppraisalResponseDto>>> Submit([FromBody] AppraisalSubmissionDto dto)
        {
            try
            {
                var result = await _appraisalService.SubmitAppraisalAsync(dto);
                return Ok(SuccessResponse<AppraisalResponseDto>.Ok(result, "Appraisal submitted successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(SuccessResponse<AppraisalResponseDto>.Fail(ex.Message, ErrorType.NotFound));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(SuccessResponse<AppraisalResponseDto>.Fail(ex.Message, ErrorType.Validation));
            }
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SuccessResponse<AppraisalResponseDto>>> GetById(long id)
        {
            try
            {
                var appraisal = await _appraisalService.GetAppraisalDetailsAsync(id);
                var response = new AppraisalResponseDto
                {
                    Id = appraisal.Id,
                    TotalScore = appraisal.TotalScore ?? 0,
                    Grade = appraisal.RatingLabel ?? "N/A"
                };
                return Ok(SuccessResponse<AppraisalResponseDto>.Ok(response, "Appraisal retrieved successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(SuccessResponse<AppraisalResponseDto>.Fail(ex.Message, ErrorType.NotFound));
            }
        }
    }
}