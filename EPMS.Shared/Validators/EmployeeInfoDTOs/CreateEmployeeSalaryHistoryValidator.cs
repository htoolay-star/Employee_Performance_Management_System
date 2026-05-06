using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeSalaryHistoryValidator : AbstractValidator<CreateEmployeeSalaryHistoryDto>
{
    public CreateEmployeeSalaryHistoryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0.");

        RuleFor(x => x.PreviousAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Previous amount must be greater than or equal to 0.");

        RuleFor(x => x.NewAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("New amount must be greater than or equal to 0.");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .WithMessage("Effective date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Effective date cannot be in the future.");

        RuleFor(x => x.ChangeReason)
            .NotEmpty()
            .WithMessage("Change reason is required.")
            .MaximumLength(500)
            .WithMessage("Change reason cannot exceed 500 characters.");

        RuleFor(x => x.ApprovedById)
            .GreaterThan(0)
            .When(x => x.ApprovedById.HasValue)
            .WithMessage("Approved by ID must be greater than 0.");

        RuleFor(x => x.NewAmount)
            .NotEqual(x => x.PreviousAmount)
            .WithMessage("New amount must be different from previous amount.");
    }
}
