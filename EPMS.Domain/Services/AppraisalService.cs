using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Form;

namespace EPMS.Application.Services.Performance
{

    public class AppraisalService : IAppraisalService
    {

        private readonly IUnitOfWork _unitOfWork;


        public AppraisalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AppraisalResponseDto> SubmitAppraisalAsync(AppraisalSubmissionDto dto)
        {
            var appraisal = await _unitOfWork.Appraisals.GetAppraisalWithDetailsAsync(dto.Id);

            if (appraisal == null) throw new Exception("Appraisal not found.");

            // Update details
            foreach (var detailDto in dto.Details)
            {
                var detail = appraisal.Details.FirstOrDefault(d =>
                    (detailDto.KPIId.HasValue && d.KPIId == detailDto.KPIId) ||
                    (detailDto.QuestionId.HasValue && d.QuestionId == detailDto.QuestionId));

                if (detail != null)
                {
                    detail.Evaluate(detailDto.ActualValue, detailDto.Rating, detailDto.Comment);
                }
            }

            // Calculate score and find matching rating scale
            var totalScore = appraisal.Details.Sum(d => d.WeightedScore);
            var scales = await _unitOfWork.HR.RatingScales.GetAllAsync();
            var matchingScale = scales.FirstOrDefault(s => s.IsMatch(totalScore));

            if (matchingScale != null)
            {
                appraisal.CalculateTotalScore(matchingScale);
            }

            _unitOfWork.Appraisals.Update(appraisal);
            await _unitOfWork.CompleteAsync();

            return new AppraisalResponseDto
            {
                Id = appraisal.Id,
                TotalScore = appraisal.TotalScore ?? 0,
                Grade = appraisal.RatingLabel ?? "N/A"
            };
        }


        public async Task<Appraisal?> GetAppraisalDetailsAsync(long id)
        {
            return await _unitOfWork.Appraisals.GetAppraisalWithDetailsAsync(id);
        }
    }
}