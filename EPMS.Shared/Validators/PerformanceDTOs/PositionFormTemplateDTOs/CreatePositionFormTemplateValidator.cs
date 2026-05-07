using EPMS.Shared.DTOs.PerformanceDTOs.PositionFormTemplateDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PositionFormTemplateDTOs;

public class CreatePositionFormTemplateValidator : AbstractValidator<CreatePositionFormTemplateDto>
{
    public CreatePositionFormTemplateValidator()
    {
        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage("Position ID must be greater than 0.");

        RuleFor(x => x.FormTemplateId)
            .GreaterThan(0)
            .WithMessage("Form template ID must be greater than 0.");
    }
}