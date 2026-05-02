using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.DepartmentDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.HrValidators
{
    public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(HrValidationMessages.Department.CodeRequired)
                .MaximumLength(20).WithMessage(HrValidationMessages.Department.CodeMaxLength);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(HrValidationMessages.Department.NameRequired)
                .MaximumLength(100).WithMessage(HrValidationMessages.Department.NameMaxLength);
        }
    }
}
