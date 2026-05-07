using EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.ContinuousFeedbackDTOs
{
    public class UpdateContinuousFeedbackValidator : AbstractValidator<UpdateContinuousFeedbackDto>
    {
        public UpdateContinuousFeedbackValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.ContinuousFeedback.IdRequired);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage(PerformanceValidationMessages.ContinuousFeedback.ContentRequired)
                .MaximumLength(2000).WithMessage(PerformanceValidationMessages.ContinuousFeedback.ContentMaxLength);

            RuleFor(x => x.Visibility)
                .NotEmpty().WithMessage(PerformanceValidationMessages.ContinuousFeedback.VisibilityRequired)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.ContinuousFeedback.VisibilityMaxLength);
        }
    }
}