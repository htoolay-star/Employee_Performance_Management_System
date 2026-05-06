using EPMS.Shared.Constants.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class PermissionValidationExtensions
{
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
}