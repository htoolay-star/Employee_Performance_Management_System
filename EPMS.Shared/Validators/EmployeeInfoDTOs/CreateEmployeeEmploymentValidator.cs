using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeEmploymentValidator : AbstractValidator<CreateEmployeeEmploymentDto>
{
    public CreateEmployeeEmploymentValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.DepartmentIdInvalid);

        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.PositionIdInvalid);

        RuleFor(x => x.EmploymentStatus)
            .ApplyEmploymentStatusRules();

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
            .Must(s => string.IsNullOrEmpty(s) || StaffTypes.All.Contains(s))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.StaffTypeInvalid);

        RuleFor(x => x.ProbationMonth)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.ProbationMonthInvalid);

        RuleFor(x => x.Shift)
            .Must(s => string.IsNullOrEmpty(s) || Shift.All.Contains(s))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.ShiftInvalid);

        RuleFor(x => x.DateOfAppointment)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.DateOfAppointmentFuture);

        RuleFor(x => x.FingerPrintId)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.FingerPrintIdMaxLength);

        RuleFor(x => x.ProductProject)
            .MaximumLength(200)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.ProductProjectMaxLength);
    }
}
