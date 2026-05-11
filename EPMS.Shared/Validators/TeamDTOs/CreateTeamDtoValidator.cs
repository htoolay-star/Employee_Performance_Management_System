using EPMS.Shared.DTOs.TeamDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.TeamDTOs;

public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.Code).ApplyTeamCodeRules();
        RuleFor(x => x.Name).ApplyTeamNameRules();
        RuleFor(x => x.Description).ApplyTeamOptionalDescriptionRules();
        RuleFor(x => x.DepartmentId).ApplyTeamDepartmentIdRules();
        RuleFor(x => x.LeadTeamId).ApplyTeamLeadTeamIdRules();
    }
}
