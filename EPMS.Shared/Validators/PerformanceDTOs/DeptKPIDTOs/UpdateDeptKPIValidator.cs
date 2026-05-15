using EPMS.Shared.DTOs.Performance.DeptKPI;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.DeptKPIDTOs
{
    public class UpdateDeptKPIValidator : AbstractValidator<UpdateDeptKPIDto>
    {
        public UpdateDeptKPIValidator()
        {
            RuleFor(x => x.PriorityId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.DeptKPI.PriorityIdRequired);
            RuleFor(x => x.Weightage)
                .InclusiveBetween(0, 100).WithMessage(PerformanceValidationMessages.DeptKPI.WeightageInvalid);
            RuleFor(x => x.TargetValue)
                .MaximumLength(100).WithMessage(PerformanceValidationMessages.DeptKPI.TargetValueMaxLength);
            RuleFor(x => x.TargetUnit)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.DeptKPI.TargetUnitMaxLength);
        }
    }
}
