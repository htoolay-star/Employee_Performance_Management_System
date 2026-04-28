using EPMS.Shared.Constants;
using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.Auth;
using EPMS.Shared.Enums.EPMS.Shared.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Validators.AuthValidators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Email).ApplyEmailRules();

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage(AuthValidationMessages.User.RoleInvalid)
                .Must(role => AuthConstants.AppRoles.AssignableRoles.Contains(role.ToString()))
                .WithMessage(AuthValidationMessages.User.SystemAdminNotAllowed);
        }
    }
}
