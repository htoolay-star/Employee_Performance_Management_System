using EPMS.Shared.DTOs.Auth;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthDTOs;

public class AdminResetPasswordRequestValidator : AbstractValidator<AdminResetPasswordRequest>
{
    public AdminResetPasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword).ApplyPasswordRules();
        RuleFor(x => x.ConfirmPassword).ApplyConfirmMatches(x => x.NewPassword);
    }
}
