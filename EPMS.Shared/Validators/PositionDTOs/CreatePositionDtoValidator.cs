using EPMS.Shared.DTOs.PositionDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.PositionDTOs;

public class CreatePositionDtoValidator : AbstractValidator<CreatePositionDto>
{
    public CreatePositionDtoValidator()
    {
        RuleFor(x => x.Title).ApplyPositionTitleRules();
        RuleFor(x => x.LevelId).ApplyPositionLevelIdRules();
    }
}
