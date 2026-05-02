using EPMS.Shared.DTOs.Auth;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthDTOs;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email).ApplyEmailRules();
    }
}
