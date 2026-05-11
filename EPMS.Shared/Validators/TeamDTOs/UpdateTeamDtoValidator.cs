using EPMS.Shared.DTOs.TeamDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.TeamDTOs;

public class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
{
    public UpdateTeamDtoValidator()
    {
        RuleFor(x => x.Name).ApplyTeamNameRules();
        
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage(HrValidationMessages.Team.DepartmentIdInvalid)
            .When(x => x.DepartmentId.HasValue);
    }
}
