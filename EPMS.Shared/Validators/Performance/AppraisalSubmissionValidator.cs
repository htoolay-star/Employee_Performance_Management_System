using FluentValidation;
using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.FormDTOs;

namespace EPMS.Application.Validations.Performance
{
    public class AppraisalSubmissionValidator : AbstractValidator<AppraisalSubmissionDto>
    {
        public AppraisalSubmissionValidator()
        {
            // Appraisal ID must exist
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(PerformanceValidationMessages.Appraisal.IdRequired);

            // Evaluator Information
            RuleFor(x => x.EvaluatorId).GreaterThan(0).WithMessage(PerformanceValidationMessages.Appraisal.EvaluatorIdRequired);
            RuleFor(x => x.EvaluatorRole).NotEmpty().WithMessage(PerformanceValidationMessages.Appraisal.EvaluatorRoleRequired);

            // Details must not be empty
            RuleFor(x => x.Details).NotEmpty().WithMessage(PerformanceValidationMessages.Appraisal.DetailsRequired);

            // Validate each detail in the list
            RuleForEach(x => x.Details).SetValidator(new AppraisalDetailValidator());
        }
    }

    public class AppraisalDetailValidator : AbstractValidator<AppraisalDetailDto>
    {
        public AppraisalDetailValidator()
        {
            // Rating check (1 to 5)
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage(PerformanceValidationMessages.Appraisal.RatingRange);

            // Comment length check
            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage(PerformanceValidationMessages.Appraisal.CommentMaxLength);
        }
    }
}