using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeSalaryHistoryValidator : AbstractValidator<CreateEmployeeSalaryHistoryDto>
{
    public CreateEmployeeSalaryHistoryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmployeeIdInvalid);

        RuleFor(x => x.PreviousAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.PreviousAmountInvalid);

        RuleFor(x => x.NewAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.NewAmountInvalid);

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.EffectiveDateRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.EffectiveDateFuture);

        RuleFor(x => x.ChangeReason)
            .NotEmpty()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.ChangeReasonRequired)
            .MaximumLength(500)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.ChangeReasonMaxLength);

        RuleFor(x => x.ApprovedById)
            .GreaterThan(0)
            .When(x => x.ApprovedById.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.ApprovedByIdInvalid);

        RuleFor(x => x.NewAmount)
            .NotEqual(x => x.PreviousAmount)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.NewAmountDifferent);
    }
}
