using FluentValidation;
using EPMS.Shared.DTOs.Form;

namespace EPMS.Application.Validations.Performance
{
    public class AppraisalSubmissionValidator : AbstractValidator<AppraisalSubmissionDto>
    {
        public AppraisalSubmissionValidator()
        {
            // Appraisal ID must exist
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Appraisal ID is required.");

            // Evaluator Information
            RuleFor(x => x.EvaluatorId).GreaterThan(0).WithMessage("Evaluator ID is required.");
            RuleFor(x => x.EvaluatorRole).NotEmpty().WithMessage("Evaluator Role is required.");

            // Details must not be empty
            RuleFor(x => x.Details).NotEmpty().WithMessage("Assessment scores are required.");

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
                .WithMessage("Rating must be between 1 and 5.");

            // Comment length check
            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters.");
        }
    }
}