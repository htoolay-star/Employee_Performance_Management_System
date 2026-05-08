using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class UpdateAppraisalValidator : AbstractValidator<UpdateAppraisalDto>
{
    public UpdateAppraisalValidator()
    {
        RuleFor(x => x.Status)
            .MaximumLength(50)
            .WithMessage(PerformanceValidationMessages.Appraisal.StatusMaxLength);

        RuleFor(x => x.EmployeeComment)
            .MaximumLength(1000)
            .WithMessage(PerformanceValidationMessages.Appraisal.CommentMaxLength);

        RuleFor(x => x.ManagerComment)
            .MaximumLength(1000)
            .WithMessage(PerformanceValidationMessages.Appraisal.CommentMaxLength);

        RuleFor(x => x.RatingLabel)
            .MaximumLength(100)
            .WithMessage("Rating label cannot exceed 100 characters.");
    }
}