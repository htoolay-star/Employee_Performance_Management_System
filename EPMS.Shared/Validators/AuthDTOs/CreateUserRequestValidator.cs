using EPMS.Shared.DTOs.Auth;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthDTOs;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email).ApplyEmailRules();
        RuleFor(x => x.StaffNo)
            .NotEmpty().WithMessage("Staff No is required.");
        RuleFor(x => x.StaffName)
            .NotEmpty().WithMessage("Staff Name is required.");
        RuleFor(x => x.PositionId)
            .GreaterThan(0).WithMessage("Position is required.");
    }
}
