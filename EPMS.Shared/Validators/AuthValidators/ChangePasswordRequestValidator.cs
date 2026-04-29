using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.AuthDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Validators.AuthValidators
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator() 
        {
            RuleFor(x => x.CurrentPassword).ApplyPasswordRules();

            RuleFor(x => x.NewPassword).ApplyPasswordRules();

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(AuthValidationMessages.Password.ConfirmRequired)
                    .Equal(x => x.NewPassword).WithMessage(AuthValidationMessages.Password.Mismatch);
        }
    }
}
