using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService;
using EPMS.Shared.DTOs.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // 1. Fetch Appraisal Record with Details
            var appraisal = await _unitOfWork.Performance.Appraisals.GetAppraisalWithDetailsAsync(dto.Id);

            // Business Logic: All checks are done here in Service
            if (appraisal == null)
                throw new KeyNotFoundException($"Appraisal with ID {dto.Id} was not found.");

            if (appraisal.IsLocked)
                throw new InvalidOperationException("This appraisal is already finalized/locked and cannot be modified.");

            // 2. Process each rating from the DTO
            foreach (var detailDto in dto.Details)
            {
                // Find matching question in the appraisal details
                var detail = appraisal.Details.FirstOrDefault(d =>
                    (detailDto.KPIId.HasValue && d.KPIId == detailDto.KPIId) ||
                    (detailDto.QuestionId.HasValue && d.QuestionId == detailDto.QuestionId));

                if (detail != null)
                {
                    // detail.Evaluate handles the internal WeightedScore calculation based on Domain logic
                    detail.Evaluate(detailDto.ActualValue, detailDto.Rating, detailDto.Comment);
                }
            }

            // 3. Final Score Calculation (ACE Data System Standard)
            // TotalScore = Sum of all weighted scores from details
            var currentTotalScore = appraisal.Details.Sum(d => d.WeightedScore);

            // Get grading scales from HR configuration (e.g., 86-100 = Outstanding)
            var scales = await _unitOfWork.HR.RatingScales.GetAllAsync();
            var matchingScale = scales.FirstOrDefault(s =>
                currentTotalScore >= s.MinScore && currentTotalScore <= s.MaxScore);

            if (matchingScale != null)
            {
                // This updates TotalScore and RatingLabel inside the entity
                appraisal.CalculateTotalScore(matchingScale);
            }

            // 4. Persistence - Update record and commit transaction
            _unitOfWork.Performance.Appraisals.Update(appraisal);
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
            var result = await _unitOfWork.Performance.Appraisals.GetAppraisalWithDetailsAsync(id);

            if (result == null)
                throw new KeyNotFoundException($"No data found for Appraisal ID {id}");

            return result;
        }
    }
}