using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class UpdateEmployeeEmploymentValidator : AbstractValidator<UpdateEmployeeEmploymentDto>
{
    public UpdateEmployeeEmploymentValidator()
    {
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

        RuleFor(x => x.EmploymentStatus)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmploymentStatusMaxLength);

        RuleFor(x => x.StaffType)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.StaffTypeMaxLength);

        RuleFor(x => x.ProbationMonth)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ProbationMonth.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.ProbationMonthInvalid);

        RuleFor(x => x.DateOfAppointment)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.DateOfAppointment.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.DateOfAppointmentFuture);

        RuleFor(x => x.DateOfConfirmation)
            .GreaterThan(x => x.DateOfAppointment)
            .When(x => x.DateOfConfirmation.HasValue && x.DateOfAppointment.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.DateOfConfirmationAfterAppointment);

        RuleFor(x => x.DateOfIncrement)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.DateOfIncrement.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.DateOfIncrementFuture);

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
