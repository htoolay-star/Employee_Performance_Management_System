using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators;

public static class AppValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyNotificationTitleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AppValidationMessages.Notification.TitleRequired)
            .MaximumLength(200).WithMessage(AppValidationMessages.Notification.TitleMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyNotificationMessageRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AppValidationMessages.Notification.MessageRequired)
            .MaximumLength(1000).WithMessage(AppValidationMessages.Notification.MessageMaxLength);
    }

    public static IRuleBuilderOptions<T, string> ApplyNotificationTypeRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(AppValidationMessages.Notification.TypeRequired)
            .MaximumLength(50).WithMessage(AppValidationMessages.Notification.TypeMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> ApplyOptionalUrlRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(500).WithMessage(AppValidationMessages.Notification.UrlMaxLength)
            .Must(x => string.IsNullOrWhiteSpace(x) || Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage(AppValidationMessages.Notification.UrlInvalid);
    }
}