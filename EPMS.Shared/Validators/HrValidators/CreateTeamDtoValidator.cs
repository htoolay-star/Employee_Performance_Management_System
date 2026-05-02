using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.TeamDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.HrValidators
{
    public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
    {
        public CreateTeamDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(HrValidationMessages.Team.NameRequired)
                .MaximumLength(100).WithMessage(HrValidationMessages.Team.NameMaxLength);

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage(HrValidationMessages.Team.DepartmentIdInvalid);
        }
    }
}
