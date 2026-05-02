using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.FormDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class AppraisalSubmissionValidator : AbstractValidator<AppraisalSubmissionDto>
{
    public AppraisalSubmissionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(PerformanceValidationMessages.Appraisal.IdRequired);

        RuleFor(x => x.EvaluatorId).GreaterThan(0).WithMessage(PerformanceValidationMessages.Appraisal.EvaluatorIdRequired);
        RuleFor(x => x.EvaluatorRole).NotEmpty().WithMessage(PerformanceValidationMessages.Appraisal.EvaluatorRoleRequired);

        RuleFor(x => x.Details).NotEmpty().WithMessage(PerformanceValidationMessages.Appraisal.DetailsRequired);

        RuleForEach(x => x.Details).SetValidator(new AppraisalDetailDtoValidator());
    }
}
