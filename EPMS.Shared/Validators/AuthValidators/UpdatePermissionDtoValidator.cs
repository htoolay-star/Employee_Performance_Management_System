using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.Auth;
using FluentValidation;

namespace EPMS.Shared.Validators.AuthValidators
{
    public class UpdatePermissionDtoValidator : AbstractValidator<UpdatePermissionDto>
    {
        public UpdatePermissionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(PermissionValidationMessages.Name.Required)
                .MaximumLength(100).WithMessage(PermissionValidationMessages.Name.MaxLength);
        }
    }
}
