using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.QuestionRatingScaleDTOs;

public class CreateQuestionRatingScaleValidator : AbstractValidator<CreateQuestionRatingScaleDto>
{
    public CreateQuestionRatingScaleValidator()
    {
        RuleFor(x => x.Name).ApplyQuestionRatingScaleNameRules();
        RuleFor(x => x.MinScore).ApplyMinScoreRules();
        RuleFor(x => x.MaxScore).ApplyMaxScoreRules();
        RuleFor(x => x.MaxScore)
            .GreaterThan(x => x.MinScore)
            .WithMessage("Maximum score must be greater than minimum score.");
    }
}