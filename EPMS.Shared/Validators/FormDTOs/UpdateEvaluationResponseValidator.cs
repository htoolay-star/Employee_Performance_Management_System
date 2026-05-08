using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class UpdateEvaluationResponseValidator : AbstractValidator<UpdateEvaluationResponseDto>
{
    public UpdateEvaluationResponseValidator()
    {
        RuleFor(x => x.RatingValue)
            .InclusiveBetween(1, 5)
            .When(x => x.RatingValue.HasValue)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.RatingValueInvalid);

        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.CommentMaxLength);
    }
}