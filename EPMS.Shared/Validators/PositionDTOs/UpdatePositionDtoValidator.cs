using EPMS.Shared.DTOs.PositionDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.PositionDTOs;

public class UpdatePositionDtoValidator : AbstractValidator<UpdatePositionDto>
{
    public UpdatePositionDtoValidator()
    {
        RuleFor(x => x.Code).ApplyPositionCodeRules();
        RuleFor(x => x.Name).ApplyPositionNameRules();
        RuleFor(x => x.LevelId).ApplyPositionLevelIdRules();
        RuleFor(x => x.Description).ApplyPositionDescriptionRules();
    }
}
