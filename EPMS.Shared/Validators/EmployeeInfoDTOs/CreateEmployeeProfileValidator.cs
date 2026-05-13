using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class CreateEmployeeProfileValidator : AbstractValidator<CreateEmployeeProfileDto>
{
    public CreateEmployeeProfileValidator()
    {
        RuleFor(x => x.StaffNo)
            .ApplyStaffNoRules();

        RuleFor(x => x.StaffName)
            .ApplyPersonNameRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.StaffNameRequired);

        RuleFor(x => x.OtherName)
            .ApplyOptionalPersonNameRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.OtherNameMaxLength);

        RuleFor(x => x.NRCNo)
            .ApplyNRCRules();

        RuleFor(x => x.Gender)
            .Must(g => string.IsNullOrEmpty(g) || Genders.All.Contains(g))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.GenderInvalid);

        RuleFor(x => x.Religion)
            .Must(r => string.IsNullOrEmpty(r) || Religions.All.Contains(r))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.ReligionInvalid);

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.DateOfBirthFuture);

        RuleFor(x => x.Nationality)
            .ApplyOptionalPersonNameRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.NationalityMaxLength);

        RuleFor(x => x.WorkPermitNo)
            .ApplyNRCRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.WorkPermitNoMaxLength);

        RuleFor(x => x.ProfilePictureUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.ProfilePictureUrlInvalid);

        RuleFor(x => x.ProfileThumbnailUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.ProfileThumbnailUrlInvalid);

        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.EmailAddressRequired)
            .MaximumLength(100)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.EmailAddressMaxLength)
            .EmailAddress()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.EmailAddressInvalid);
    }
}
