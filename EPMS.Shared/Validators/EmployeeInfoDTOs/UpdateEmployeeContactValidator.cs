using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class UpdateEmployeeContactValidator : AbstractValidator<UpdateEmployeeContactDto>
{
    public UpdateEmployeeContactValidator()
    {
        RuleFor(x => x.ContactAddress)
            .MaximumLength(500)
            .WithMessage("Contact address cannot exceed 500 characters.");

        RuleFor(x => x.PermanentAddress)
            .MaximumLength(500)
            .WithMessage("Permanent address cannot exceed 500 characters.");

        RuleFor(x => x.PhoneNo)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters.")
            .Matches(@"^[+]?[\d\s\-\(\)]+$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNo))
            .WithMessage("Phone number format is invalid.");

        RuleFor(x => x.PermanentPhoneNo)
            .MaximumLength(20)
            .WithMessage("Permanent phone number cannot exceed 20 characters.")
            .Matches(@"^[+]?[\d\s\-\(\)]+$")
            .When(x => !string.IsNullOrEmpty(x.PermanentPhoneNo))
            .WithMessage("Permanent phone number format is invalid.");

        RuleFor(x => x.PresentPhoneNo)
            .MaximumLength(20)
            .WithMessage("Present phone number cannot exceed 20 characters.")
            .Matches(@"^[+]?[\d\s\-\(\)]+$")
            .When(x => !string.IsNullOrEmpty(x.PresentPhoneNo))
            .WithMessage("Present phone number format is invalid.");

        RuleFor(x => x.EmailAddress)
            .MaximumLength(100)
            .WithMessage("Email address cannot exceed 100 characters.")
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.EmailAddress))
            .WithMessage("Email address format is invalid.");

        RuleFor(x => x.InternalPhoneNo)
            .MaximumLength(20)
            .WithMessage("Internal phone number cannot exceed 20 characters.");

        RuleFor(x => x.EmergencyMobileNo)
            .MaximumLength(20)
            .WithMessage("Emergency mobile number cannot exceed 20 characters.")
            .Matches(@"^[+]?[\d\s\-\(\)]+$")
            .When(x => !string.IsNullOrEmpty(x.EmergencyMobileNo))
            .WithMessage("Emergency mobile number format is invalid.");

        RuleFor(x => x.RelationWithEmergencyContact)
            .MaximumLength(50)
            .WithMessage("Relation with emergency contact cannot exceed 50 characters.");
    }
}
