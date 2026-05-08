using EPMS.Shared.DTOs.Performance.PositionKPI;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PositionKPIDTOs
{
    public class CreatePositionKPIValidator : AbstractValidator<CreatePositionKPIDto>
    {
        public CreatePositionKPIValidator()
        {
            RuleFor(x => x.PositionId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.PositionKPI.PositionIdRequired);
            RuleFor(x => x.KPIId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.PositionKPI.KPIIdRequired);
            RuleFor(x => x.PriorityId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.PositionKPI.PriorityIdRequired);
            RuleFor(x => x.Weightage)
                .InclusiveBetween(0, 100).WithMessage(PerformanceValidationMessages.PositionKPI.WeightageInvalid);
            RuleFor(x => x.TargetValue)
                .MaximumLength(100).WithMessage(PerformanceValidationMessages.PositionKPI.TargetValueMaxLength);
            RuleFor(x => x.TargetUnit)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.PositionKPI.TargetUnitMaxLength);
        }
    }
}