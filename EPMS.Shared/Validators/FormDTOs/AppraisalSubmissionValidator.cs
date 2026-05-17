using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class AppraisalSubmissionValidator : AbstractValidator<AppraisalSubmissionDto>
{
    public AppraisalSubmissionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(PerformanceValidationMessages.Appraisal.IdRequired);

        RuleFor(x => x.Details).NotEmpty().WithMessage(PerformanceValidationMessages.Appraisal.DetailsRequired);

        RuleForEach(x => x.Details).SetValidator(new AppraisalDetailDtoValidator());
    }
}
