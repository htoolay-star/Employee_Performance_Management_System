using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.PermissionDTOS;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthValidators
{
    public class CreatePermissionDtoValidator : AbstractValidator<CreatePermissionDto>
    {
        public CreatePermissionDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(PermissionValidationMessages.Code.Required)
                .MaximumLength(50).WithMessage(PermissionValidationMessages.Code.MaxLength);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(PermissionValidationMessages.Name.Required)
                .MaximumLength(100).WithMessage(PermissionValidationMessages.Name.MaxLength);
        }
    }
}
