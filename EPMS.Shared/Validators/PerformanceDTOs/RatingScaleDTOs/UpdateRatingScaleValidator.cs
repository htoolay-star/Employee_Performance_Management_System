using EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.RatingScaleDTOs;

public class UpdateRatingScaleValidator : AbstractValidator<UpdateRatingScaleDto>
{
    public UpdateRatingScaleValidator()
    {
        RuleFor(x => x.MinScore)
            .ApplyOptionalScoreRangeRules("Minimum", cmd => cmd.MinScore);

        RuleFor(x => x.MaxScore)
            .ApplyOptionalScoreRangeRules("Maximum", cmd => cmd.MaxScore);

        RuleFor(x => x.MinScore)
            .LessThanOrEqualTo(x => x.MaxScore)
            .When(x => x.MinScore.HasValue && x.MaxScore.HasValue)
            .WithMessage(PerformanceValidationMessages.RatingScale.MinScoreGreaterThanMax);

        RuleFor(x => x.PromotionEligibility)
            .MaximumLength(100)
            .WithMessage(PerformanceValidationMessages.RatingScale.PromotionEligibilityMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage(PerformanceValidationMessages.RatingScale.DescriptionMaxLength);
    }
}
