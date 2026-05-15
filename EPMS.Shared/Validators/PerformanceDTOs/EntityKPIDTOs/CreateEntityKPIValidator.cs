using EPMS.Shared.DTOs.Performance.EntityKPI;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.EntityKPIDTOs
{
    public class CreateEntityKPIValidator : AbstractValidator<CreateEntityKPIDto>
    {
        public CreateEntityKPIValidator()
        {
            RuleFor(x => x.EntityType)
                .NotEmpty().WithMessage(PerformanceValidationMessages.EntityKPI.EntityTypeRequired)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.EntityKPI.EntityTypeMaxLength);
            RuleFor(x => x.EntityId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.EntityKPI.EntityIdRequired);
            RuleFor(x => x.KPIId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.EntityKPI.KPIIdRequired);
            RuleFor(x => x.PriorityId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.EntityKPI.PriorityIdRequired);
            RuleFor(x => x.Weightage)
                .InclusiveBetween(0, 100).WithMessage(PerformanceValidationMessages.EntityKPI.WeightageInvalid);
            RuleFor(x => x.TargetValue)
                .MaximumLength(100).WithMessage(PerformanceValidationMessages.EntityKPI.TargetValueMaxLength);
            RuleFor(x => x.TargetUnit)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.EntityKPI.TargetUnitMaxLength);
        }
    }
}
