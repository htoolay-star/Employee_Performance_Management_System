using System;
using System.Linq.Expressions;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class PerformanceValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyHexColorCodeRules<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        Func<T, string?> propertySelector)
    {
        return ruleBuilder
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage(PerformanceValidationMessages.KPIWeightPriority.ColorCodeInvalid)
            .When(x => !string.IsNullOrWhiteSpace(propertySelector(x)));
    }

    public static IRuleBuilderOptions<T, int> ApplyRatingRules<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder.GreaterThan(0).WithMessage(PerformanceValidationMessages.RatingScale.RatingInvalid);
    }

    public static IRuleBuilderOptions<T, decimal> ApplyScoreRangeRules<T>(this IRuleBuilder<T, decimal> ruleBuilder, string scoreType)
    {
        var message = scoreType switch
        {
            "Minimum" => PerformanceValidationMessages.ScoreRange.MinimumScoreInvalid,
            "Maximum" => PerformanceValidationMessages.ScoreRange.MaximumScoreInvalid,
            _ => PerformanceValidationMessages.ScoreRange.MinimumScoreInvalid
        };
        return ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage(message);
    }

    public static IRuleBuilderOptions<T, decimal?> ApplyOptionalScoreRangeRules<T>(
        this IRuleBuilder<T, decimal?> ruleBuilder,
        string scoreType,
        Func<T, decimal?> propertySelector)
    {
        var message = scoreType switch
        {
            "Minimum" => PerformanceValidationMessages.ScoreRange.MinimumScoreInvalid,
            "Maximum" => PerformanceValidationMessages.ScoreRange.MaximumScoreInvalid,
            _ => PerformanceValidationMessages.ScoreRange.MinimumScoreInvalid
        };
        return ruleBuilder
            .GreaterThanOrEqualTo(0)
            .WithMessage(message)
            .When(x => propertySelector(x).HasValue);
    }

    public static IRuleBuilderOptions<T, decimal> ApplyWeightRangeRules<T>(this IRuleBuilder<T, decimal> ruleBuilder, string weightType)
    {
        var message = weightType switch
        {
            "Minimum" => PerformanceValidationMessages.WeightRange.MinimumWeightInvalid,
            "Maximum" => PerformanceValidationMessages.WeightRange.MaximumWeightInvalid,
            _ => PerformanceValidationMessages.WeightRange.MinimumWeightInvalid
        };
        return ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage(message);
    }

    public static IRuleBuilderOptions<T, decimal?> ApplyOptionalyWeightRangeRules<T>(
        this IRuleBuilder<T, decimal?> ruleBuilder,
        string weightType,
        Func<T, decimal?> propertySelector)
    {
        var message = weightType switch
        {
            "Minimum" => PerformanceValidationMessages.WeightRange.MinimumWeightInvalid,
            "Maximum" => PerformanceValidationMessages.WeightRange.MaximumWeightInvalid,
            _ => PerformanceValidationMessages.WeightRange.MinimumWeightInvalid
        };
        return ruleBuilder
            .GreaterThanOrEqualTo(0)
            .WithMessage(message)
            .When(x => propertySelector(x).HasValue);
    }

    public static IRuleBuilderOptions<T, string> ApplyQuestionRatingScaleNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PerformanceValidationMessages.QuestionRatingScale.NameRequired)
            .MaximumLength(100).WithMessage(PerformanceValidationMessages.QuestionRatingScale.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalQuestionRatingScaleNameRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(100).WithMessage(PerformanceValidationMessages.QuestionRatingScale.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, decimal> ApplyMinScoreRules<T>(this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage(PerformanceValidationMessages.QuestionRatingScale.MinScoreInvalid);
    }

    public static IRuleBuilderOptions<T, decimal> ApplyMaxScoreRules<T>(this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage(PerformanceValidationMessages.QuestionRatingScale.MaxScoreInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyPositionPIPTemplateTitleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PerformanceValidationMessages.PositionPIPTemplate.TitleRequired)
            .MaximumLength(200).WithMessage(PerformanceValidationMessages.PositionPIPTemplate.TitleMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPositionPIPTemplateTitleRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(200).WithMessage(PerformanceValidationMessages.PositionPIPTemplate.TitleMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyPositionPIPTemplateSuccessCriteriaRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PerformanceValidationMessages.PositionPIPTemplate.SuccessCriteriaRequired)
            .MaximumLength(1000).WithMessage(PerformanceValidationMessages.PositionPIPTemplate.SuccessCriteriaMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPositionPIPTemplateSuccessCriteriaRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(1000).WithMessage(PerformanceValidationMessages.PositionPIPTemplate.SuccessCriteriaMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPositionPIPTemplateDescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(500).WithMessage(PerformanceValidationMessages.PositionPIPTemplate.DescriptionMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyPositionFormTemplateTitleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Position form template title is required.")
            .MaximumLength(200).WithMessage("Position form template title cannot exceed 200 characters.");
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPositionFormTemplateTitleRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(200).WithMessage("Position form template title cannot exceed 200 characters.");
    }

    public static IRuleBuilderOptions<T, string> ApplyPositionFormTemplateSuccessCriteriaRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Position form template success criteria is required.")
            .MaximumLength(1000).WithMessage("Position form template success criteria cannot exceed 1000 characters.");
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPositionFormTemplateSuccessCriteriaRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(1000).WithMessage("Position form template success criteria cannot exceed 1000 characters.");
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPositionFormTemplateDescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(500).WithMessage("Position form template description cannot exceed 500 characters.");
    }

    public static IRuleBuilderOptions<T, string> ApplyPIPObjectiveTitleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PerformanceValidationMessages.PIPObjective.TitleRequired)
            .MaximumLength(200).WithMessage(PerformanceValidationMessages.PIPObjective.TitleMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPIPObjectiveTitleRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(200).WithMessage(PerformanceValidationMessages.PIPObjective.TitleMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyPIPObjectiveSuccessCriteriaRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PerformanceValidationMessages.PIPObjective.SuccessCriteriaRequired)
            .MaximumLength(1000).WithMessage(PerformanceValidationMessages.PIPObjective.SuccessCriteriaMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPIPObjectiveSuccessCriteriaRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(1000).WithMessage(PerformanceValidationMessages.PIPObjective.SuccessCriteriaMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPIPObjectiveManagerCommentRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(500).WithMessage(PerformanceValidationMessages.PIPObjective.ManagerCommentMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPIPObjectiveStatusRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.PIPObjective.StatusMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalPIPObjectiveDescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(500).WithMessage(PerformanceValidationMessages.PIPObjective.DescriptionMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyFormQuestionTextRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PerformanceValidationMessages.FormQuestion.QuestionTextRequired)
            .MaximumLength(500).WithMessage(PerformanceValidationMessages.FormQuestion.QuestionTextMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalFormQuestionTextRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(500).WithMessage(PerformanceValidationMessages.FormQuestion.QuestionTextMaxLength);
    }

    public static IRuleBuilderOptions<T, long?> ApplyOptionalFormQuestionCategoryRules<T>(this IRuleBuilder<T, long?> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage(PerformanceValidationMessages.FormQuestion.CategoryIdInvalid);
    }

    public static IRuleBuilderOptions<T, long?> ApplyOptionalFormQuestionRatingScaleRules<T>(this IRuleBuilder<T, long?> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage(PerformanceValidationMessages.FormQuestion.RatingScaleIdInvalid);
    }
}