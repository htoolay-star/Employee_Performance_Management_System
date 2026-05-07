using EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.KPIWeightPriorityDTOs;

public class CreateKPIWeightPriorityValidator : AbstractValidator<CreateKPIWeightPriorityDto>
{
    public CreateKPIWeightPriorityValidator()
    {
        RuleFor(x => x.LevelName)
            .ApplyLevelNameRules();

        RuleFor(x => x.MinWeight)
            .ApplyWeightRangeRules("Minimum")
            .WithMessage(PerformanceValidationMessages.KPIWeightPriority.MinWeightInvalid);

        RuleFor(x => x.MaxWeight)
            .ApplyWeightRangeRules("Maximum")
            .WithMessage(PerformanceValidationMessages.KPIWeightPriority.MaxWeightInvalid);

        RuleFor(x => x.MinWeight)
            .LessThanOrEqualTo(x => x.MaxWeight)
            .WithMessage(PerformanceValidationMessages.KPIWeightPriority.MinWeightGreaterThanMax);

        RuleFor(x => x.ColorCode)
            .ApplyHexColorCodeRules(cmd => cmd.ColorCode);
    }
}
