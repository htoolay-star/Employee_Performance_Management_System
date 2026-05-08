using EPMS.Shared.DTOs.FormDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.FormDTOs;

public class CreateEvaluationResponseValidator : AbstractValidator<CreateEvaluationResponseDto>
{
    public CreateEvaluationResponseValidator()
    {
        RuleFor(x => x.AppraisalId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.AppraisalIdRequired);

        RuleFor(x => x.TemplateId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.TemplateIdRequired);

        RuleFor(x => x.QuestionId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.QuestionIdRequired);

        RuleFor(x => x.EvaluatorId)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.EvaluatorIdRequired);

        RuleFor(x => x.EvaluatorRole)
            .NotEmpty()
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.EvaluatorRoleRequired)
            .MaximumLength(50)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.EvaluatorRoleMaxLength);

        RuleFor(x => x.RatingValue)
            .InclusiveBetween(1, 5)
            .When(x => x.RatingValue.HasValue)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.RatingValueInvalid);

        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .WithMessage(PerformanceValidationMessages.EvaluationResponse.CommentMaxLength);
    }
}