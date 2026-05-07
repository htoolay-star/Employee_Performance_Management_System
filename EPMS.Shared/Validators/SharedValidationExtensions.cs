using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class SharedValidationExtensions
{
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

    public static IRuleBuilderOptions<T, long?> ApplyCategoryOptionalParentIdRules<T>(this IRuleBuilder<T, long?> ruleBuilder)
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