using EPMS.Shared.DTOs.Performance.EntityKPI;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.EntityKPIDTOs
{
    public class UpdateEntityKPIValidator : AbstractValidator<UpdateEntityKPIDto>
    {
        public UpdateEntityKPIValidator()
        {
            RuleFor(x => x.PriorityId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.EntityKPI.PriorityIdRequired);
            RuleFor(x => x.Weightage)
                .InclusiveBetween(0, 100).WithMessage(PerformanceValidationMessages.EntityKPI.WeightageInvalid);
            RuleFor(x => x.TargetValue)
                .InclusiveBetween(0, 9999999999m).WithMessage(PerformanceValidationMessages.EntityKPI.TargetValueInvalid);
            RuleFor(x => x.TargetUnit)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.EntityKPI.TargetUnitMaxLength);
        }
    }
}
