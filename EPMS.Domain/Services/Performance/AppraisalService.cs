using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Form;
using EPMS.Shared.Enums;

namespace EPMS.Domain.Services.Performance
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

            if (appraisal == null)
                throw new KeyNotFoundException($"Appraisal ID {dto.Id} not found.");

            if (appraisal.IsLocked)
                throw new InvalidOperationException("This record is finalized and cannot be modified.");

            
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

            
            var totalPoints = appraisal.Details.Sum(d => d.Score); // Rating values
            var questionCount = appraisal.Details.Count(d => d.Score > 0);

            decimal finalScore = 0;
            if (questionCount > 0)
            {
                finalScore = (totalPoints * 100m) / (questionCount * 5);
            }

            
            var scales = await _unitOfWork.HR.RatingScales.GetAllAsync();
            var matchingScale = scales.FirstOrDefault(s => finalScore >= s.MinScore && finalScore <= s.MaxScore);

           
            if (matchingScale != null)
            {
                appraisal.CalculateTotalScore(matchingScale);
            }

            
            var type = typeof(Appraisal);
            type.GetProperty("TotalScore")?.SetValue(appraisal, finalScore);

            appraisal.FinalizeAppraisal();

           
            _unitOfWork.Appraisals.Update(appraisal);
            await _unitOfWork.CompleteAsync();

            return new AppraisalResponseDto
            {
                Id = appraisal.Id,
                TotalScore = finalScore,
                Grade = appraisal.RatingLabel ?? "N/A"
            };
        }

        public async Task<Appraisal?> GetAppraisalDetailsAsync(long id)
        {
           
            return await _unitOfWork.Appraisals.GetAppraisalWithDetailsAsync(id);
        }
    }
}