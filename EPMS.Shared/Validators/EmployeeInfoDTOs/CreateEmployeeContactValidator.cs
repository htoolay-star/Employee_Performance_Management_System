using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeContactValidator : AbstractValidator<CreateEmployeeContactDto>
{
    public CreateEmployeeContactValidator()
    {
        RuleFor(x => x.EmployeeId)
            .ApplyEmployeeIdRules();

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

        RuleFor(x => x.EmailAddress)
            .ApplyOptionalEmailAddressRules(cmd => cmd.EmailAddress);

        RuleFor(x => x.InternalPhoneNo)
            .MaximumLength(20)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.InternalPhoneNoMaxLength);

        RuleFor(x => x.EmergencyMobileNo)
            .ApplyOptionalPhoneNumberRules(cmd => cmd.PhoneNo)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.EmergencyMobileNoMaxLength);

        RuleFor(x => x.RelationWithEmergencyContact)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.RelationWithEmergencyContactMaxLength);
    }
}
