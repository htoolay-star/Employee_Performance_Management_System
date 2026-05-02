using EPMS.Shared.DTOs.Auth;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthDTOs;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).ApplyEmailRules();
        RuleFor(x => x.Password).ApplyPasswordRules();
    }
}
