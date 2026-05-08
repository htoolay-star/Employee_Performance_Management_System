using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class CreateAppraisalValidator : AbstractValidator<CreateAppraisalDto>
{
    public CreateAppraisalValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.Appraisal.EmployeeIdRequired);

        RuleFor(x => x.CycleId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.Appraisal.CycleIdRequired);

        RuleFor(x => x.AppraiserId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.Appraisal.AppraiserIdRequired);

        RuleFor(x => x.EvaluatorRole)
            .NotEmpty()
            .WithMessage(PerformanceValidationMessages.Appraisal.EvaluatorRoleRequired)
            .MaximumLength(50)
            .WithMessage(PerformanceValidationMessages.Appraisal.EvaluatorRoleMaxLength);
    }
}