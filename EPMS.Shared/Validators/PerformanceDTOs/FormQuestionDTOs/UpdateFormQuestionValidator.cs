using EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;
using EPMS.Shared.Validators;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.FormQuestionDTOs;

public class UpdateFormQuestionValidator : AbstractValidator<UpdateFormQuestionDto>
{
    public UpdateFormQuestionValidator()
    {
        RuleFor(x => x.QuestionText)
            .ApplyOptionalFormQuestionTextRules();

        RuleFor(x => x.Sequence)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.FormQuestion.SequenceInvalid)
            .When(x => x.Sequence.HasValue);

        RuleFor(x => x.CategoryId)
            .ApplyOptionalFormQuestionCategoryRules();
    }
}