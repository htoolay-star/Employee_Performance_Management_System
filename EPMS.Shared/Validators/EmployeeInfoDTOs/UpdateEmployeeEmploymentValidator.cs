using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class UpdateEmployeeEmploymentValidator : AbstractValidator<UpdateEmployeeEmploymentDto>
{
    public UpdateEmployeeEmploymentValidator()
    {
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

        RuleFor(x => x.EmploymentStatus)
            .MaximumLength(50)
            .WithMessage("Employment status cannot exceed 50 characters.");

        RuleFor(x => x.StaffType)
            .MaximumLength(50)
            .WithMessage("Staff type cannot exceed 50 characters.");

        RuleFor(x => x.ProbationMonth)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ProbationMonth.HasValue)
            .WithMessage("Probation months must be greater than or equal to 0.");

        RuleFor(x => x.DateOfAppointment)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.DateOfAppointment.HasValue)
            .WithMessage("Date of appointment cannot be in the future.");

        RuleFor(x => x.DateOfConfirmation)
            .GreaterThan(x => x.DateOfAppointment)
            .When(x => x.DateOfConfirmation.HasValue && x.DateOfAppointment.HasValue)
            .WithMessage("Date of confirmation must be after date of appointment.");

        RuleFor(x => x.DateOfIncrement)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.DateOfIncrement.HasValue)
            .WithMessage("Date of increment cannot be in the future.");

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
