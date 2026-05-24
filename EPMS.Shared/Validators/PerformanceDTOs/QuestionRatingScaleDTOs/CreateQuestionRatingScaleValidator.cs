using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.QuestionRatingScaleDTOs;

public class CreateQuestionRatingScaleValidator : AbstractValidator<CreateQuestionRatingScaleDto>
{
    public CreateQuestionRatingScaleValidator()
    {
        RuleFor(x => x.Name).ApplyQuestionRatingScaleNameRules();

        RuleFor(x => x.Levels)
            .NotEmpty()
            .WithMessage("At least one rating level is required.");

        RuleForEach(x => x.Levels)
            .SetValidator(new CreateQuestionRatingScaleLevelValidator());

        RuleFor(x => x.Levels)
            .Must(levels => levels.Select(l => l.Rating).Distinct().Count() == levels.Count)
            .WithMessage("Rating values must be unique.");

        RuleFor(x => x.Levels)
            .Must(HasNoOverlappingRanges)
            .WithMessage("Score ranges must not overlap.");
    }

    private bool HasNoOverlappingRanges(List<CreateQuestionRatingScaleLevelDto> levels)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            for (int j = i + 1; j < levels.Count; j++)
            {
                if (levels[i].MinScore <= levels[j].MaxScore && levels[j].MinScore <= levels[i].MaxScore)
                    return false;
            }
        }
        return true;
    }
}
