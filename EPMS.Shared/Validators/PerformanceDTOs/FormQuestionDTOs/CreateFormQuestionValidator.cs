using EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;
using EPMS.Shared.Validators;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.FormQuestionDTOs;

public class CreateFormQuestionValidator : AbstractValidator<CreateFormQuestionDto>
{
    public CreateFormQuestionValidator()
    {
        RuleFor(x => x.TemplateId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.FormQuestion.TemplateIdRequired);

        RuleFor(x => x.QuestionText)
            .ApplyFormQuestionTextRules();

        RuleFor(x => x.Sequence)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.FormQuestion.SequenceInvalid);

        RuleFor(x => x.CategoryId)
            .ApplyOptionalFormQuestionCategoryRules();

        RuleFor(x => x.RatingScaleId)
            .ApplyOptionalFormQuestionRatingScaleRules();
    }
}