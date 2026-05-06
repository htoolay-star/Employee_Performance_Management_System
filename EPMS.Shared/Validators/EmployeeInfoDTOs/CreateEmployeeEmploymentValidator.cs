using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeEmploymentValidator : AbstractValidator<CreateEmployeeEmploymentDto>
{
    public CreateEmployeeEmploymentValidator()
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

        RuleFor(x => x.ParentDepartmentId)
            .GreaterThan(0)
            .WithMessage("Parent department ID must be greater than 0.");

        RuleFor(x => x.TeamId)
            .GreaterThan(0)
            .When(x => x.TeamId.HasValue)
            .WithMessage("Team ID must be greater than 0.");

        RuleFor(x => x.DirectManagerId)
            .GreaterThan(0)
            .When(x => x.DirectManagerId.HasValue)
            .WithMessage("Direct manager ID must be greater than 0.");

        RuleFor(x => x.StaffType)
            .MaximumLength(50)
            .WithMessage("Staff type cannot exceed 50 characters.");

        RuleFor(x => x.ProbationMonth)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Probation months must be greater than or equal to 0.");

        RuleFor(x => x.Shift)
            .MaximumLength(50)
            .WithMessage("Shift cannot exceed 50 characters.");

        RuleFor(x => x.FingerPrintId)
            .MaximumLength(50)
            .WithMessage("Fingerprint ID cannot exceed 50 characters.");

        RuleFor(x => x.ProductProject)
            .MaximumLength(200)
            .WithMessage("Product/project cannot exceed 200 characters.");
    }
}
