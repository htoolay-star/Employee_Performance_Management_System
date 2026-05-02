using EPMS.Shared.DTOs.CategoryDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.CategoryDTOs;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Module).ApplyCategoryModuleRules();
        RuleFor(x => x.Code).ApplyCategoryCodeRules();
        RuleFor(x => x.Name).ApplyCategoryNameRules();
        RuleFor(x => x.Description).ApplyCategoryOptionalDescriptionRules();
        RuleFor(x => x.ParentId).ApplyCategoryOptionalParentIdRules();
    }
}
