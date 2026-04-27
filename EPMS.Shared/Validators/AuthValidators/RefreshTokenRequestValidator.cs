using EPMS.Shared.DTOs.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Validators.AuthValidators
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required to generate a new session.");
        }
    }
}
