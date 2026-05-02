using EPMS.Shared.DTOs.PositionDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.PositionDTOs;

public class UpdatePositionDtoValidator : AbstractValidator<UpdatePositionDto>
{
    public UpdatePositionDtoValidator()
    {
        RuleFor(x => x.Title).ApplyPositionTitleRules();
        RuleFor(x => x.LevelId).ApplyPositionLevelIdRules();
    }
}
