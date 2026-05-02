using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.FormDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class AppraisalDetailDtoValidator : AbstractValidator<AppraisalDetailDto>
{
    public AppraisalDetailDtoValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage(PerformanceValidationMessages.Appraisal.RatingRange);

        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .WithMessage(PerformanceValidationMessages.Appraisal.CommentMaxLength);
    }
}
