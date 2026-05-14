using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeContactValidator : AbstractValidator<CreateEmployeeContactDto>
{
    public CreateEmployeeContactValidator()
    {
        RuleFor(x => x.ContactAddress)
            .ApplyAddressRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.ContactAddressMaxLength);

        RuleFor(x => x.PermanentAddress)
            .ApplyAddressRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PermanentAddressMaxLength);

        RuleFor(x => x.PhoneNo)
            .ApplyOptionalPhoneNumberRules(cmd => cmd.PhoneNo);

        RuleFor(x => x.PermanentPhoneNo)
            .ApplyOptionalPhoneNumberRules(cmd => cmd.PhoneNo);

        RuleFor(x => x.PresentPhoneNo)
            .ApplyOptionalPhoneNumberRules(cmd => cmd.PhoneNo);

        RuleFor(x => x.InternalPhoneNo)
            .MaximumLength(20)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.InternalPhoneNoMaxLength);

        RuleFor(x => x.EmergencyMobileNo)
            .ApplyOptionalPhoneNumberRules(cmd => cmd.PhoneNo)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.EmergencyMobileNoMaxLength);

        RuleFor(x => x.RelationWithEmergencyContact)
            .Must(r => string.IsNullOrEmpty(r) || RelationWithEmergencyContact.All.Contains(r))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.RelationWithEmergencyContactInvalid);
    }
}
