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

    public static IRuleBuilderOptions<T, string> ApplyPerformanceLevelNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PerformanceValidationMessages.KPIWeightPriority.LevelNameRequired)
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.KPIWeightPriority.LevelNameMaxLength);
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
}