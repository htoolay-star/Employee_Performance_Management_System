using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.QuestionRatingScaleDTOs;

public class UpdateQuestionRatingScaleValidator : AbstractValidator<UpdateQuestionRatingScaleDto>
{
    public UpdateQuestionRatingScaleValidator()
    {
        RuleFor(x => x.Name).ApplyOptionalQuestionRatingScaleNameRules();

        When(x => x.Levels != null && x.Levels.Any(), () =>
        {
            RuleFor(x => x.Levels)
                .Must(levels => levels!.Select(l => l.Rating).Distinct().Count() == levels.Count)
                .WithMessage("Rating values must be unique.");

            RuleFor(x => x.Levels)
                .Must(HasNoOverlappingRanges)
                .WithMessage("Score ranges must not overlap.");

            RuleForEach(x => x.Levels)
                .SetValidator(new UpdateQuestionRatingScaleLevelValidator());
        });
    }

    private bool HasNoOverlappingRanges(List<UpdateQuestionRatingScaleLevelDto>? levels)
    {
        if (levels == null || levels.Count <= 1) return true;

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
