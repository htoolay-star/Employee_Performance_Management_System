using System.Linq.Expressions;
using EPMS.Shared.Constants.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyEmailRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AuthValidationMessages.Email.Required)
            .MaximumLength(256).WithMessage(AuthValidationMessages.Email.MaxLength)
            .EmailAddress().WithMessage(AuthValidationMessages.Email.Invalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AuthValidationMessages.Password.Required)
            .MinimumLength(8).WithMessage(AuthValidationMessages.Password.MinimumLength)
            .Must(p => !p.Any(char.IsWhiteSpace)).WithMessage(AuthValidationMessages.Password.NoSpaces)
            .Must(p => p.Any(char.IsUpper)).WithMessage(AuthValidationMessages.Password.RequiresUppercase)
            .Must(p => p.Any(char.IsLower)).WithMessage(AuthValidationMessages.Password.RequiresLowercase)
            .Must(p => p.Any(char.IsDigit)).WithMessage(AuthValidationMessages.Password.RequiresDigit);
    }

    public static IRuleBuilderOptions<T, string> ApplyConfirmMatches<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        Expression<Func<T, string>> matchTo)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AuthValidationMessages.Password.ConfirmRequired)
            .Equal(matchTo).WithMessage(AuthValidationMessages.Password.Mismatch);
    }

    public static IRuleBuilderOptions<T, string> ApplyRefreshTokenRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AuthValidationMessages.Tokens.RefreshTokenRequired);
    }

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

    public static IRuleBuilderOptions<T, int> ApplyPositionLevelIdRules<T>(this IRuleBuilder<T, int> ruleBuilder)
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

    public static IRuleBuilderOptions<T, string> ApplyPermissionCodeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PermissionValidationMessages.Code.Required)
            .MaximumLength(50).WithMessage(PermissionValidationMessages.Code.MaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyPermissionNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(PermissionValidationMessages.Name.Required)
            .MaximumLength(100).WithMessage(PermissionValidationMessages.Name.MaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyCategoryModuleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.Category.ModuleRequired)
            .MaximumLength(50).WithMessage(SharedValidationMessages.Category.ModuleMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyCategoryCodeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.Category.CodeRequired)
            .MaximumLength(50).WithMessage(SharedValidationMessages.Category.CodeMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyCategoryNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.Category.NameRequired)
            .MaximumLength(100).WithMessage(SharedValidationMessages.Category.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyCategoryOptionalDescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(s => string.IsNullOrWhiteSpace(s) || s.Length <= 250)
            .WithMessage(SharedValidationMessages.Category.DescriptionMaxLength);
    }

    public static IRuleBuilderOptions<T, int?> ApplyCategoryOptionalParentIdRules<T>(this IRuleBuilder<T, int?> ruleBuilder)
    {
        return ruleBuilder
            .Must(pid => !pid.HasValue || pid.Value > 0)
            .WithMessage(SharedValidationMessages.Category.ParentIdInvalid);
    }

    public static IRuleBuilderOptions<T, string> ApplyTagNameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(SharedValidationMessages.Tag.NameRequired)
            .MaximumLength(50).WithMessage(SharedValidationMessages.Tag.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyTagOptionalModuleRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(s => s == null || string.IsNullOrWhiteSpace(s) || s.Length <= 50)
            .WithMessage(SharedValidationMessages.Tag.ModuleMaxLength);
    }
}
