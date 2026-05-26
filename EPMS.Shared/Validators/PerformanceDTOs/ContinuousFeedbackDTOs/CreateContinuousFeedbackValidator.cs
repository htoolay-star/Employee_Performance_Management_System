using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.ContinuousFeedbackDTOs
{
    public class CreateContinuousFeedbackValidator : AbstractValidator<CreateContinuousFeedbackDto>
    {
        public CreateContinuousFeedbackValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.ContinuousFeedback.EmployeeIdRequired);

            RuleFor(x => x.GivenById)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.ContinuousFeedback.GivenByIdRequired);

            RuleFor(x => x.FeedbackType)
                .NotEmpty().WithMessage(PerformanceValidationMessages.ContinuousFeedback.FeedbackTypeRequired)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.ContinuousFeedback.FeedbackTypeMaxLength)
                .Must(v => ContinuousFeedbackTypes.All.Contains(v))
                .WithMessage(PerformanceValidationMessages.ContinuousFeedback.FeedbackTypeInvalid);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage(PerformanceValidationMessages.ContinuousFeedback.ContentRequired)
                .MaximumLength(2000).WithMessage(PerformanceValidationMessages.ContinuousFeedback.ContentMaxLength);

            RuleFor(x => x.Visibility)
                .NotEmpty().WithMessage(PerformanceValidationMessages.ContinuousFeedback.VisibilityRequired)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.ContinuousFeedback.VisibilityMaxLength);
        }
    }
}