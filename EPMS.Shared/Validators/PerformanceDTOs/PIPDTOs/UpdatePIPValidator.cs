using EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.PIPDTOs
{
    public class UpdatePIPValidator : AbstractValidator<UpdatePIPDto>
    {
        public UpdatePIPValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.PIP.IdRequired);

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