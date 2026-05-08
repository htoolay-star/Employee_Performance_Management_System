using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.QuestionRatingScaleDTOs;

public class UpdateQuestionRatingScaleValidator : AbstractValidator<UpdateQuestionRatingScaleDto>
{
    public UpdateQuestionRatingScaleValidator()
    {
        RuleFor(x => x.Name).ApplyOptionalQuestionRatingScaleNameRules();
        
        When(x => x.MinScore.HasValue, () =>
        {
            RuleFor(x => x.MinScore!.Value).ApplyMinScoreRules();
        });
        
        When(x => x.MaxScore.HasValue, () =>
        {
            RuleFor(x => x.MaxScore!.Value).ApplyMaxScoreRules();
        });
        
        RuleFor(x => x.MaxScore)
            .GreaterThan(x => x.MinScore ?? 0)
            .WithMessage("Maximum score must be greater than minimum score.")
            .When(x => x.MaxScore.HasValue && x.MinScore.HasValue);
    }
}