using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeePayrollInfoValidator : AbstractValidator<CreateEmployeePayrollInfoDto>
{
    public CreateEmployeePayrollInfoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmployeeIdInvalid);

        RuleFor(x => x.Salary)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.SalaryInvalid);

        RuleFor(x => x.Currency)
            .Must(c => string.IsNullOrEmpty(c) || Currency.All.Contains(c))
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.CurrencyInvalid);

        RuleFor(x => x.PayType)
            .Must(p => string.IsNullOrEmpty(p) || PayType.All.Contains(p))
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.PayTypeInvalid);

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
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.ComplianceEarnedPointsInvalid);

        RuleFor(x => x.ComplianceBalancePoints)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeePayrollInfo.ComplianceBalancePointsInvalid);
    }
}
