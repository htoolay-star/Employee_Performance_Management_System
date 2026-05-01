using EPMS.Shared.DTOs.AuthDTOs;
using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.Validators.AuthValidators
{
    public class UpdateDefaultPasswordValidator : AbstractValidator<UpdateDefaultPasswordRequest>
    {
        public UpdateDefaultPasswordValidator() 
        {
            RuleFor(x => x.NewDefaultPassword).ApplyPasswordRules();
        }
    }
}
