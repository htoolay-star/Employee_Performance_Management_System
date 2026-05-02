using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using FluentValidation;

namespace EPMS.Shared.Validators.PermissionDTOS;

public class CreatePermissionDtoValidator : AbstractValidator<CreatePermissionDto>
{
    public CreatePermissionDtoValidator()
    {
        RuleFor(x => x.Code).ApplyPermissionCodeRules();
        RuleFor(x => x.Name).ApplyPermissionNameRules();
    }
}
