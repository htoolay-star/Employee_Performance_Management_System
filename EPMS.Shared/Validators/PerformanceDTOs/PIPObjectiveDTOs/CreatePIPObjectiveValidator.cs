using EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;
using EPMS.Shared.Validators;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PIPObjectiveDTOs;

public class CreatePIPObjectiveValidator : AbstractValidator<CreatePIPObjectiveDto>
{
    public CreatePIPObjectiveValidator()
    {
        RuleFor(x => x.PIPId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.PIPObjective.PIPIdRequired);

        RuleFor(x => x.Title)
            .ApplyPIPObjectiveTitleRules();

        RuleFor(x => x.SuccessCriteria)
            .ApplyPIPObjectiveSuccessCriteriaRules();

        RuleFor(x => x.Description)
            .ApplyOptionalPIPObjectiveDescriptionRules();
    }
}