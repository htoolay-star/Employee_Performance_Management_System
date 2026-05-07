using System.Linq.Expressions;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class AuthValidationExtensions
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
}