using EPMS.Shared.DTOs.DepartmentDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.DepartmentDTOs;

public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentDtoValidator()
    {
        RuleFor(x => x.Code).ApplyDepartmentCodeRules();
        RuleFor(x => x.Name).ApplyDepartmentNameRules();
    }
}
