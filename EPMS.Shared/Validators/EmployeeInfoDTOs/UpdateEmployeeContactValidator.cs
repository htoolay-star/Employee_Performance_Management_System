using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class UpdateEmployeeContactValidator : AbstractValidator<UpdateEmployeeContactDto>
{
    public UpdateEmployeeContactValidator()
    {
        RuleFor(x => x.ContactAddress)
            .MaximumLength(500)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.ContactAddressMaxLength);

        RuleFor(x => x.PermanentAddress)
            .MaximumLength(500)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PermanentAddressMaxLength);

        RuleFor(x => x.PhoneNo)
            .MaximumLength(20)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberMaxLength)
            .Matches(@"^[+]?[\d\s\-\(\)]+$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNo))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberInvalid);

        RuleFor(x => x.PermanentPhoneNo)
            .MaximumLength(20)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberMaxLength)
            .Matches(@"^[+]?[\d\s\-\(\)]+$")
            .When(x => !string.IsNullOrEmpty(x.PermanentPhoneNo))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberInvalid);

        RuleFor(x => x.PresentPhoneNo)
            .MaximumLength(20)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberMaxLength)
            .Matches(@"^[+]?[\d\s\-\(\)]+$")
            .When(x => !string.IsNullOrEmpty(x.PresentPhoneNo))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberInvalid);

        RuleFor(x => x.InternalPhoneNo)
            .MaximumLength(20)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.InternalPhoneNoMaxLength);

        RuleFor(x => x.EmergencyMobileNo)
            .MaximumLength(20)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.EmergencyMobileNoMaxLength)
            .Matches(@"^[+]?[\d\s\-\(\)]+$")
            .When(x => !string.IsNullOrEmpty(x.EmergencyMobileNo))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.PhoneNumberInvalid);

        RuleFor(x => x.RelationWithEmergencyContact)
            .Must(r => string.IsNullOrEmpty(r) || RelationWithEmergencyContact.All.Contains(r))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeContact.RelationWithEmergencyContactInvalid);
    }
}
