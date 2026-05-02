using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.DepartmentDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.HrValidators
{
    public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(HrValidationMessages.Department.NameRequired)
                .MaximumLength(100).WithMessage(HrValidationMessages.Department.NameMaxLength);
        }
    }
}
