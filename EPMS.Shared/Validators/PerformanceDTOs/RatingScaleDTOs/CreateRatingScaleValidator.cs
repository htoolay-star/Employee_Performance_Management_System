using EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.RatingScaleDTOs;

public class CreateRatingScaleValidator : AbstractValidator<CreateRatingScaleDto>
{
    public CreateRatingScaleValidator()
    {
        RuleFor(x => x.Rating)
            .ApplyRatingRules();

        RuleFor(x => x.Label)
            .NotEmpty()
            .WithMessage(PerformanceValidationMessages.RatingScale.LabelRequired)
            .MaximumLength(100)
            .WithMessage(PerformanceValidationMessages.RatingScale.LabelMaxLength);

        RuleFor(x => x.MinScore)
            .ApplyScoreRangeRules("Minimum")
            .WithMessage(PerformanceValidationMessages.RatingScale.MinScoreInvalid);

        RuleFor(x => x.MaxScore)
            .ApplyScoreRangeRules("Maximum")
            .WithMessage(PerformanceValidationMessages.RatingScale.MaxScoreInvalid);

        RuleFor(x => x.MinScore)
            .LessThanOrEqualTo(x => x.MaxScore)
            .WithMessage(PerformanceValidationMessages.RatingScale.MinScoreGreaterThanMax);

        RuleFor(x => x.PromotionEligibility)
            .MaximumLength(100)
            .WithMessage(PerformanceValidationMessages.RatingScale.PromotionEligibilityMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage(PerformanceValidationMessages.RatingScale.DescriptionMaxLength);
    }
}
