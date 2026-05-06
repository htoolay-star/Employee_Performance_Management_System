using EPMS.Shared.Constants.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class HrValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyDepartmentCodeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Department.CodeRequired)
            .MaximumLength(20).WithMessage(HrValidationMessages.Department.CodeMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyDepartmentNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Department.NameRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Department.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyTeamNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Team.NameRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Team.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, long> ApplyTeamDepartmentIdRules<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder.GreaterThan(0).WithMessage(HrValidationMessages.Team.DepartmentIdInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyPositionTitleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Position.TitleRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Position.TitleMaxLength);
    }

    public static IRuleBuilderOptions<T, long> ApplyPositionLevelIdRules<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder.GreaterThan(0).WithMessage(HrValidationMessages.Position.LevelIdInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyLevelCodeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Level.CodeRequired)
            .MaximumLength(10).WithMessage(HrValidationMessages.Level.CodeMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyLevelNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(HrValidationMessages.Level.NameRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Level.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyLevelOptionalDescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(s => string.IsNullOrWhiteSpace(s) || s.Length <= 250)
            .WithMessage(HrValidationMessages.Level.DescriptionMaxLength);
    }
}