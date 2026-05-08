using EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PIPObjectiveDTOs;

public class UpdatePIPObjectiveValidator : AbstractValidator<UpdatePIPObjectiveDto>
{
    public UpdatePIPObjectiveValidator()
    {
        RuleFor(x => x.Title)
            .ApplyOptionalPIPObjectiveTitleRules();

        RuleFor(x => x.SuccessCriteria)
            .ApplyOptionalPIPObjectiveSuccessCriteriaRules();

        RuleFor(x => x.Description)
            .ApplyOptionalPIPObjectiveDescriptionRules();

        RuleFor(x => x.Status)
            .ApplyOptionalPIPObjectiveStatusRules();

        RuleFor(x => x.ManagerComment)
            .ApplyOptionalPIPObjectiveManagerCommentRules();
    }
}