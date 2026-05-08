using EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PositionPIPTemplateDTOs;

public class UpdatePositionPIPTemplateValidator : AbstractValidator<UpdatePositionPIPTemplateDto>
{
    public UpdatePositionPIPTemplateValidator()
    {
        RuleFor(x => x.Title)
            .ApplyOptionalPositionPIPTemplateTitleRules();

        RuleFor(x => x.SuccessCriteria)
            .ApplyOptionalPositionPIPTemplateSuccessCriteriaRules();

        RuleFor(x => x.Description)
            .ApplyOptionalPositionPIPTemplateDescriptionRules();
    }
}