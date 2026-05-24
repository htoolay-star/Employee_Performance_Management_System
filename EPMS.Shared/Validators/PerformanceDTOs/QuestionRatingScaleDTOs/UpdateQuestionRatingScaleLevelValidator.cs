using EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.QuestionRatingScaleDTOs;

public class UpdateQuestionRatingScaleLevelValidator : AbstractValidator<UpdateQuestionRatingScaleLevelDto>
{
    public UpdateQuestionRatingScaleLevelValidator()
    {
        RuleFor(x => x.Rating)
            .GreaterThan(0)
            .WithMessage("Rating must be greater than 0.");

        RuleFor(x => x.MinScore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum score must be zero or greater.");

        RuleFor(x => x.MaxScore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maximum score must be zero or greater.");

        RuleFor(x => x.MaxScore)
            .GreaterThanOrEqualTo(x => x.MinScore)
            .WithMessage("Maximum score must be greater than or equal to minimum score.");
    }
}
