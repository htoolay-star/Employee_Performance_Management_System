using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class UpdateAppraisalRecommendationValidator : AbstractValidator<UpdateAppraisalRecommendationDto>
{
    public UpdateAppraisalRecommendationValidator()
    {
        RuleFor(x => x.Type)
            .MaximumLength(50)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.TypeMaxLength);

        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.ReasonMaxLength);

        RuleFor(x => x.ProposedValue)
            .MaximumLength(100)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.ProposedValueMaxLength);

        RuleFor(x => x.Priority)
            .MaximumLength(20)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.PriorityMaxLength);
    }
}
