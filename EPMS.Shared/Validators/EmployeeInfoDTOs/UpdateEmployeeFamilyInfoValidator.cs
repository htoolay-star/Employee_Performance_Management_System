using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class UpdateEmployeeFamilyInfoValidator : AbstractValidator<UpdateEmployeeFamilyInfoDto>
{
    public UpdateEmployeeFamilyInfoValidator()
    {
        RuleFor(x => x.MaritalStatus)
            .MaximumLength(50)
            .WithMessage("Marital status cannot exceed 50 characters.");

        RuleFor(x => x.SpouseName)
            .MaximumLength(100)
            .WithMessage("Spouse name cannot exceed 100 characters.");

        RuleFor(x => x.SpouseNRCNo)
            .MaximumLength(50)
            .WithMessage("Spouse NRC number cannot exceed 50 characters.");

        RuleFor(x => x.SpouseOccupation)
            .MaximumLength(100)
            .WithMessage("Spouse occupation cannot exceed 100 characters.");

        RuleFor(x => x.FatherName)
            .MaximumLength(100)
            .WithMessage("Father name cannot exceed 100 characters.");

        RuleFor(x => x.FatherNRCNo)
            .MaximumLength(50)
            .WithMessage("Father NRC number cannot exceed 50 characters.");

        RuleFor(x => x.FatherOccupation)
            .MaximumLength(100)
            .WithMessage("Father occupation cannot exceed 100 characters.");
    }
}
