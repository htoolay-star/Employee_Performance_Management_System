using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class CreateAppraisalValidator : AbstractValidator<CreateAppraisalDto>
{
    public CreateAppraisalValidator()
    {
        RuleFor(x => x.CycleId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.Appraisal.CycleIdRequired);

        RuleFor(x => x.ManagerReviewerId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.Appraisal.ManagerReviewerIdRequired);

        When(x => x.EmployeeId.HasValue, () =>
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.Appraisal.EmployeeIdRequired);

            RuleFor(x => x.EntityType).Null().WithMessage("EntityType must be null for employee appraisals.");
            RuleFor(x => x.EntityId).Null().WithMessage("EntityId must be null for employee appraisals.");
        });

        When(x => !x.EmployeeId.HasValue, () =>
        {
            RuleFor(x => x.EntityType)
                .NotEmpty()
                .WithMessage("EntityType is required when EmployeeId is not provided.");

            RuleFor(x => x.EntityId)
                .GreaterThan(0)
                .WithMessage("EntityId is required when EmployeeId is not provided.");
        });
    }
}
