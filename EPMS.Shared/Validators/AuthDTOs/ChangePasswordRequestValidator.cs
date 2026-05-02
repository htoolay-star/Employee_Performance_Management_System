using EPMS.Shared.DTOs.AuthDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthDTOs;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword).ApplyPasswordRules();
        RuleFor(x => x.NewPassword).ApplyPasswordRules();
        RuleFor(x => x.ConfirmPassword).ApplyConfirmMatches(x => x.NewPassword);
    }
}
