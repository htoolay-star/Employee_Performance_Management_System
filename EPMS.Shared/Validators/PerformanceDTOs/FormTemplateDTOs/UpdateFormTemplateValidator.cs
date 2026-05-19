using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.FormTemplateDTOs
{
    public class UpdateFormTemplateValidator : AbstractValidator<UpdateFormTemplateDto>
    {
        public UpdateFormTemplateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.FormTemplate.IdRequired);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(PerformanceValidationMessages.FormTemplate.NameRequired)
                .MaximumLength(100).WithMessage(PerformanceValidationMessages.FormTemplate.NameMaxLength);

            RuleFor(x => x.FormType)
                .NotEmpty().WithMessage(PerformanceValidationMessages.FormTemplate.FormTypeRequired)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.FormTemplate.FormTypeMaxLength)
                .Must(x => AppraisalConstants.FormTypes.All.Contains(x))
                .WithMessage(PerformanceValidationMessages.FormTemplate.FormTypeInvalid);

            RuleFor(x => x.RatingScaleId)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.FormTemplate.RatingScaleIdRequired)
                .When(x => x.RatingScaleId.HasValue);

            RuleFor(x => x.QuestionsPerEvaluation)
                .GreaterThan(0)
                .When(x => x.QuestionsPerEvaluation.HasValue)
                .WithMessage(PerformanceValidationMessages.FormTemplate.QuestionsPerEvaluationPositive);
        }
    }
}