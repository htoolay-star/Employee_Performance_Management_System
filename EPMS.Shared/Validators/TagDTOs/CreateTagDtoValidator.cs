using EPMS.Shared.DTOs.TagDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.TagDTOs;

public class CreateTagDtoValidator : AbstractValidator<CreateTagDto>
{
    public CreateTagDtoValidator()
    {
        RuleFor(x => x.Name).ApplyTagNameRules();
        RuleFor(x => x.Module).ApplyTagOptionalModuleRules();
    }
}
