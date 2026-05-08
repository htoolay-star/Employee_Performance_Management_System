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

        RuleFor(x => x.FirstName)
            .ApplyPersonNameRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.FirstNameRequired);

        RuleFor(x => x.LastName)
            .ApplyPersonNameRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.LastNameRequired);

        RuleFor(x => x.OtherName)
            .ApplyOptionalPersonNameRules()
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.OtherNameMaxLength);

        RuleFor(x => x.NRCNo)
            .ApplyNRCRules();

        RuleFor(x => x.Gender)
            .Must(g => string.IsNullOrEmpty(g) || g.Equals("Male", StringComparison.OrdinalIgnoreCase) || 
                      g.Equals("Female", StringComparison.OrdinalIgnoreCase) || g.Equals("Other", StringComparison.OrdinalIgnoreCase))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.GenderInvalid);

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
    }
}
