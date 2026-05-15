using EPMS.Shared.DTOs.Performance.TeamKPI;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.TeamKPIDTOs
{
    public class UpdateTeamKPIValidator : AbstractValidator<UpdateTeamKPIDto>
    {
        public UpdateTeamKPIValidator()
        {
            RuleFor(x => x.PriorityId)
                .GreaterThan(0).WithMessage(PerformanceValidationMessages.TeamKPI.PriorityIdRequired);
            RuleFor(x => x.Weightage)
                .InclusiveBetween(0, 100).WithMessage(PerformanceValidationMessages.TeamKPI.WeightageInvalid);
            RuleFor(x => x.TargetValue)
                .MaximumLength(100).WithMessage(PerformanceValidationMessages.TeamKPI.TargetValueMaxLength);
            RuleFor(x => x.TargetUnit)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.TeamKPI.TargetUnitMaxLength);
        }
    }
}
