using EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.KPIWeightPriorityDTOs;

public class UpdateKPIWeightPriorityValidator : AbstractValidator<UpdateKPIWeightPriorityDto>
{
    public UpdateKPIWeightPriorityValidator()
    {
        RuleFor(x => x.MinWeight)
            .ApplyOptionalyWeightRangeRules("Minimum", cmd => cmd.MinWeight);

        RuleFor(x => x.MaxWeight)
            .ApplyOptionalyWeightRangeRules("Maximum", cmd => cmd.MaxWeight);

        RuleFor(x => x.MinWeight)
            .LessThanOrEqualTo(x => x.MaxWeight)
            .When(x => x.MinWeight.HasValue && x.MaxWeight.HasValue)
            .WithMessage(PerformanceValidationMessages.KPIWeightPriority.MinWeightGreaterThanMax);

        RuleFor(x => x.ColorCode)
            .ApplyHexColorCodeRules(cmd => cmd.ColorCode);
    }
}
