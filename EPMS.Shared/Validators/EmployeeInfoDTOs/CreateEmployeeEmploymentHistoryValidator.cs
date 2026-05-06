using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeEmploymentHistoryValidator : AbstractValidator<CreateEmployeeEmploymentHistoryDto>
{
    public CreateEmployeeEmploymentHistoryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0.");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("Department ID must be greater than 0.");

        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage("Position ID must be greater than 0.");

        RuleFor(x => x.EmploymentStatus)
            .NotEmpty()
            .WithMessage("Employment status is required.")
            .MaximumLength(50)
            .WithMessage("Employment status cannot exceed 50 characters.");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .WithMessage("Effective date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Effective date cannot be in the future.");

        RuleFor(x => x.ChangeReason)
            .MaximumLength(500)
            .WithMessage("Change reason cannot exceed 500 characters.");

        RuleFor(x => x.ManagerId)
            .GreaterThan(0)
            .When(x => x.ManagerId.HasValue)
            .WithMessage("Manager ID must be greater than 0.");

        RuleFor(x => x.ChangedById)
            .GreaterThan(0)
            .When(x => x.ChangedById.HasValue)
            .WithMessage("Changed by ID must be greater than 0.");
    }
}
