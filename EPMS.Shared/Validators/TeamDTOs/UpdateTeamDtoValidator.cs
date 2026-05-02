using EPMS.Shared.DTOs.TeamDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.TeamDTOs;

public class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
{
    public UpdateTeamDtoValidator()
    {
        RuleFor(x => x.Name).ApplyTeamNameRules();
    }
}
