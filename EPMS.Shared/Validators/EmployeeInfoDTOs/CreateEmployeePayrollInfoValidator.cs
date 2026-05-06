using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeePayrollInfoValidator : AbstractValidator<CreateEmployeePayrollInfoDto>
{
    public CreateEmployeePayrollInfoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0.");

        RuleFor(x => x.Salary)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Salary must be greater than or equal to 0.");

        RuleFor(x => x.Currency)
            .MaximumLength(10)
            .WithMessage("Currency cannot exceed 10 characters.")
            .Matches(@"^[A-Z]{3}$")
            .When(x => !string.IsNullOrEmpty(x.Currency))
            .WithMessage("Currency must be a valid 3-letter currency code (e.g., USD, EUR).");

        RuleFor(x => x.PayType)
            .MaximumLength(50)
            .WithMessage("Pay type cannot exceed 50 characters.");

        RuleFor(x => x.CostAllocate)
            .MaximumLength(100)
            .WithMessage("Cost allocate cannot exceed 100 characters.");

        RuleFor(x => x.PayByBacklog)
            .MaximumLength(50)
            .WithMessage("Pay by backlog cannot exceed 50 characters.");

        RuleFor(x => x.TaxStatus)
            .MaximumLength(50)
            .WithMessage("Tax status cannot exceed 50 characters.");

        RuleFor(x => x.TaxNo)
            .MaximumLength(50)
            .WithMessage("Tax number cannot exceed 50 characters.");

        RuleFor(x => x.SSBStatus)
            .MaximumLength(50)
            .WithMessage("SSB status cannot exceed 50 characters.");

        RuleFor(x => x.SSCBNo)
            .MaximumLength(50)
            .WithMessage("SSCB number cannot exceed 50 characters.");

        RuleFor(x => x.ComplianceEarnedPoints)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Compliance earned points must be greater than or equal to 0.");

        RuleFor(x => x.ComplianceBalancePoints)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Compliance balance points must be greater than or equal to 0.");
    }
}
