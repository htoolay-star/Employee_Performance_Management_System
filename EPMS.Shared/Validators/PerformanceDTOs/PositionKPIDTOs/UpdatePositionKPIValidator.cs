using EPMS.Shared.DTOs.Performance.PositionKPI;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PositionKPIDTOs
{
    public class UpdatePositionKPIValidator : AbstractValidator<UpdatePositionKPIDto>
    {
        public UpdatePositionKPIValidator()
        {
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