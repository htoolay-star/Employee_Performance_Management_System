using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeEmploymentHistoryValidator : AbstractValidator<CreateEmployeeEmploymentHistoryDto>
{
    public CreateEmployeeEmploymentHistoryValidator()
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

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.EffectiveDateRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.EffectiveDateFuture);

        RuleFor(x => x.ChangeReason)
            .MaximumLength(500)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.ChangeReasonMaxLength);

        RuleFor(x => x.ManagerId)
            .GreaterThan(0)
            .When(x => x.ManagerId.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.ManagerIdInvalid);

        RuleFor(x => x.ChangedById)
            .GreaterThan(0)
            .When(x => x.ChangedById.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeHistory.ChangedByIdInvalid);
    }
}
