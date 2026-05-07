using EPMS.Shared.DTOs.PerformanceDTOs.PositionFormTemplateDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PositionFormTemplateDTOs;

public class UpdatePositionFormTemplateValidator : AbstractValidator<UpdatePositionFormTemplateDto>
{
    public UpdatePositionFormTemplateValidator()
    {
        RuleFor(x => x.IsMandatory)
            .NotNull()
            .WithMessage("IsMandatory is required.");
    }
}