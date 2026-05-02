using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.TeamDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.HrValidators
{
    public class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
    {
        public UpdateTeamDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(HrValidationMessages.Team.NameRequired)
                .MaximumLength(100).WithMessage(HrValidationMessages.Team.NameMaxLength);
        }
    }
}
