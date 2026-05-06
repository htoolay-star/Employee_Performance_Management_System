using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeEmploymentValidator : AbstractValidator<CreateEmployeeEmploymentDto>
{
    public CreateEmployeeEmploymentValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmployeeIdInvalid);

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.DepartmentIdInvalid);

        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.PositionIdInvalid);

        RuleFor(x => x.EmploymentStatus)
            .NotEmpty()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmploymentStatusRequired)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmploymentStatusMaxLength);

        RuleFor(x => x.ParentDepartmentId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.ParentDepartmentIdInvalid);

        RuleFor(x => x.TeamId)
            .GreaterThan(0)
            .When(x => x.TeamId.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.TeamIdInvalid);

        RuleFor(x => x.DirectManagerId)
            .GreaterThan(0)
            .When(x => x.DirectManagerId.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.DirectManagerIdInvalid);

        RuleFor(x => x.StaffType)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.StaffTypeMaxLength);

        RuleFor(x => x.ProbationMonth)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.ProbationMonthInvalid);

        RuleFor(x => x.Shift)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.ShiftMaxLength);

        RuleFor(x => x.FingerPrintId)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.FingerPrintIdMaxLength);

        RuleFor(x => x.ProductProject)
            .MaximumLength(200)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.ProductProjectMaxLength);
    }
}
