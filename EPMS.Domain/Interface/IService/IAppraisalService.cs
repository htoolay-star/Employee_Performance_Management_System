using EPMS.Domain.Entities.Performance;
using EPMS.Shared.DTOs.FormDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService
{
    public interface IAppraisalService
    {
        /// <summary>
        /// Handles the submission and calculation of Appraisal, Self-Assessment, and 360 Feedback.
        /// </summary>
        Task<AppraisalResponseDto> SubmitAppraisalAsync(AppraisalSubmissionDto dto);

        /// <summary>
        /// Retrieves a specific appraisal with all related details and questions.
        /// </summary>
        Task<Appraisal> GetAppraisalDetailsAsync(long id);
    }
}
