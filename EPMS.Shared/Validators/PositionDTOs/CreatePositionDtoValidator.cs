using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PositionDTOs;

public class CreatePositionDtoValidator : AbstractValidator<CreatePositionDto>
{
    public CreatePositionDtoValidator()
    {
        RuleFor(x => x.Code).ApplyPositionCodeRules();
        RuleFor(x => x.Name).ApplyPositionNameRules();
        RuleFor(x => x.LevelId).ApplyPositionLevelIdRules();
        RuleFor(x => x.Description).ApplyPositionDescriptionRules();
    }
}
