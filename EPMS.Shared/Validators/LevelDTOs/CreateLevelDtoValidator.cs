using EPMS.Shared.DTOs.LevelDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.LevelDTOs;

public class CreateLevelDtoValidator : AbstractValidator<CreateLevelDto>
{
    public CreateLevelDtoValidator()
    {
        RuleFor(x => x.Code).ApplyLevelCodeRules();
        RuleFor(x => x.Name).ApplyLevelNameRules();
        RuleFor(x => x.Description).ApplyLevelOptionalDescriptionRules();
    }
}
