using EPMS.Shared.DTOs.AuthDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthDTOs;

public class UpdateDefaultPasswordValidator : AbstractValidator<UpdateDefaultPasswordRequest>
{
    public UpdateDefaultPasswordValidator()
    {
        RuleFor(x => x.NewDefaultPassword).ApplyPasswordRules();
    }
}
