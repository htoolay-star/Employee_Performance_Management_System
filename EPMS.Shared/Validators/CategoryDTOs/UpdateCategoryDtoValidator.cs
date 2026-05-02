using EPMS.Shared.DTOs.CategoryDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.CategoryDTOs;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(x => x.Name).ApplyCategoryNameRules();
        RuleFor(x => x.Description).ApplyCategoryOptionalDescriptionRules();
        RuleFor(x => x.ParentId).ApplyCategoryOptionalParentIdRules();
    }
}
