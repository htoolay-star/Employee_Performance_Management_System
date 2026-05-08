using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthDTOs.PositionPermissionDTOs;

public class CreatePositionPermissionValidator : AbstractValidator<CreatePositionPermissionDto>
{
    public CreatePositionPermissionValidator()
    {
        RuleFor(x => x.PositionId)
            .GreaterThan(0)
            .WithMessage(AuthValidationMessages.PositionPermission.PositionIdRequired);

        RuleFor(x => x.PermissionId)
            .GreaterThan(0)
            .WithMessage(AuthValidationMessages.PositionPermission.PermissionIdRequired);
    }
}
