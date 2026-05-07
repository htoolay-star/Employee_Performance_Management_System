using EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PositionPIPTemplateDTOs;

public class CreatePositionPIPTemplateValidator : AbstractValidator<CreatePositionPIPTemplateDto>
{
    public CreatePositionPIPTemplateValidator()
    {
        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage("Position ID must be greater than 0.");

        RuleFor(x => x.Title)
            .ApplyPositionPIPTemplateTitleRules();

        RuleFor(x => x.SuccessCriteria)
            .ApplyPositionPIPTemplateSuccessCriteriaRules();

        RuleFor(x => x.Description)
            .ApplyOptionalPositionPIPTemplateDescriptionRules();
    }
}