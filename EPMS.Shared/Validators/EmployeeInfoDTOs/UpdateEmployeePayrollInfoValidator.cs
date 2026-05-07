using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class UpdateEmployeePayrollInfoValidator : AbstractValidator<UpdateEmployeePayrollInfoDto>
{
    public UpdateEmployeePayrollInfoValidator()
    {
        RuleFor(x => x.Salary)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.SalaryInvalid);

        RuleFor(x => x.Currency)
            .MaximumLength(10)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.CurrencyMaxLength)
            .Matches(@"^[A-Z]{3}$")
            .When(x => !string.IsNullOrEmpty(x.Currency))
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.CurrencyInvalid);

        RuleFor(x => x.PayType)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.PayTypeMaxLength);

        RuleFor(x => x.CostAllocate)
            .MaximumLength(100)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.CostAllocateMaxLength);

        RuleFor(x => x.PayByBacklog)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.PayByBacklogMaxLength);

        RuleFor(x => x.TaxStatus)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.TaxStatusMaxLength);

        RuleFor(x => x.TaxNo)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.TaxNoMaxLength);

        RuleFor(x => x.SSBStatus)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.SSBStatusMaxLength);

        RuleFor(x => x.SSCBNo)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.SSCBNoMaxLength);

        RuleFor(x => x.ComplianceEarnedPoints)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ComplianceEarnedPoints.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.ComplianceEarnedPointsInvalid);

        RuleFor(x => x.ComplianceBalancePoints)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ComplianceBalancePoints.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.ComplianceBalancePointsInvalid);
    }
}
