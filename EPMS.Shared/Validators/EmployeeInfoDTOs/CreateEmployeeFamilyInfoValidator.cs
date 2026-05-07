using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeFamilyInfoValidator : AbstractValidator<CreateEmployeeFamilyInfoDto>
{
    public CreateEmployeeFamilyInfoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeEmployment.EmployeeIdInvalid);

        RuleFor(x => x.MaritalStatus)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeFamilyInfo.MaritalStatusMaxLength);

        RuleFor(x => x.SpouseName)
            .MaximumLength(100)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeFamilyInfo.SpouseNameMaxLength);

        RuleFor(x => x.SpouseNRCNo)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeFamilyInfo.SpouseNRCMaxLength);

        RuleFor(x => x.SpouseOccupation)
            .MaximumLength(100)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeFamilyInfo.SpouseOccupationMaxLength);

        RuleFor(x => x.FatherName)
            .MaximumLength(100)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeFamilyInfo.FatherNameMaxLength);

        RuleFor(x => x.FatherNRCNo)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeFamilyInfo.FatherNRCMaxLength);

        RuleFor(x => x.FatherOccupation)
            .MaximumLength(100)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeFamilyInfo.FatherOccupationMaxLength);
    }
}
