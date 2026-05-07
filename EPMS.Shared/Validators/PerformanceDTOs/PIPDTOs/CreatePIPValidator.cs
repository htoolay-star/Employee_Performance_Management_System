using EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PIPDTOs
{
    public class CreatePIPValidator : AbstractValidator<CreatePIPDto>
    {
        public CreatePIPValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.PIP.EmployeeIdRequired);

            RuleFor(x => x.ManagerId)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.PIP.ManagerIdRequired);

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage(PerformanceValidationMessages.PIP.StartDateRequired);

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage(PerformanceValidationMessages.PIP.EndDateRequired)
                .GreaterThan(x => x.StartDate)
                .WithMessage(PerformanceValidationMessages.PIP.EndDateAfterStart);

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage(PerformanceValidationMessages.PIP.ReasonRequired)
                .MaximumLength(1000).WithMessage(PerformanceValidationMessages.PIP.ReasonMaxLength);
        }
    }
}