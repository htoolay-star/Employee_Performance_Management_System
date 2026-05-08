using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class CreateAppraisalRecommendationValidator : AbstractValidator<CreateAppraisalRecommendationDto>
{
    public CreateAppraisalRecommendationValidator()
    {
        RuleFor(x => x.AppraisalId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.AppraisalIdRequired);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.TypeRequired)
            .MaximumLength(50)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.TypeMaxLength);

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.ReasonRequired)
            .MaximumLength(500)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.ReasonMaxLength);

        RuleFor(x => x.ProposedValue)
            .MaximumLength(100)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.ProposedValueMaxLength);

        RuleFor(x => x.Priority)
            .NotEmpty()
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.PriorityRequired)
            .MaximumLength(20)
            .WithMessage(PerformanceValidationMessages.AppraisalRecommendation.PriorityMaxLength);
    }
}
