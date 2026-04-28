using EPMS.Api.Controllers;
using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Form;
using Microsoft.AspNetCore.Mvc;

namespace EPMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AppraisalController : ApiBaseController
    {
        private readonly IAppraisalService _appraisalService;

        public AppraisalController(IUnitOfWork unitOfWork, IAppraisalService appraisalService)
            : base(unitOfWork)
        {
            _appraisalService = appraisalService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit(AppraisalSubmissionDto dto)
        {
            var result = await _appraisalService.SubmitAppraisalAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _appraisalService.GetAppraisalDetailsAsync(id);
            return Ok(result);
        }
    }
}