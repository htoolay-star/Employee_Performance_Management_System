using EPMS.Shared.DTOs.DepartmentDTOs;
using EPMS.Shared.Validators;
using FluentValidation;

namespace EPMS.Shared.Validators.DepartmentDTOs;

public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
{
    public UpdateDepartmentDtoValidator()
    {
        RuleFor(x => x.Name).ApplyDepartmentNameRules();
        RuleFor(x => x.Description).ApplyDepartmentOptionalDescriptionRules();
        RuleFor(x => x.DeptHeadId).ApplyDepartmentDeptHeadIdRules();
    }
}
