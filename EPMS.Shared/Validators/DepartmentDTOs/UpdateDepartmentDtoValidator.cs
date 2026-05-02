using EPMS.Shared.DTOs.DepartmentDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.DepartmentDTOs;

public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
{
    public UpdateDepartmentDtoValidator()
    {
        RuleFor(x => x.Name).ApplyDepartmentNameRules();
    }
}
