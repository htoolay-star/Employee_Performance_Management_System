using EPMS.Shared.DTOs.LevelDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.LevelDTOs;

public class UpdateLevelDtoValidator : AbstractValidator<UpdateLevelDto>
{
    public UpdateLevelDtoValidator()
    {
        RuleFor(x => x.Name).ApplyLevelNameRules();
        RuleFor(x => x.Description).ApplyLevelOptionalDescriptionRules();
    }
}
