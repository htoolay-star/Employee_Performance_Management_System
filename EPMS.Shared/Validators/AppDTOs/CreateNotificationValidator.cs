using EPMS.Shared.DTOs.AppDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.AppDTOs;

public class CreateNotificationValidator : AbstractValidator<CreateNotificationDto>
{
    public CreateNotificationValidator()
    {
        RuleFor(x => x.ToUserId)
            .GreaterThan(0)
            .WithMessage(AppValidationMessages.Notification.ToUserIdInvalid);

        RuleFor(x => x.Title)
            .ApplyNotificationTitleRules();

        RuleFor(x => x.Message)
            .ApplyNotificationMessageRules();

        RuleFor(x => x.Type)
            .ApplyNotificationTypeRules();

        RuleFor(x => x.RedirectUrl)
            .ApplyOptionalUrlRules();
    }
}