using EPMS.Shared.DTOs.TeamDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.TeamDTOs;

public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.Name).ApplyTeamNameRules();
        RuleFor(x => x.DepartmentId).ApplyTeamDepartmentIdRules();
    }
}
