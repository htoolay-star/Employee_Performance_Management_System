using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using FluentValidation;

namespace EPMS.Shared.Validators.PermissionDTOS;

public class UpdatePermissionDtoValidator : AbstractValidator<UpdatePermissionDto>
{
    public UpdatePermissionDtoValidator()
    {
        RuleFor(x => x.Name).ApplyPermissionNameRules();
    }
}
