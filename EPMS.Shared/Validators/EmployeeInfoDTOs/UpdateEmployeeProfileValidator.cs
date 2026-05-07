using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class UpdateEmployeeProfileValidator : AbstractValidator<UpdateEmployeeProfileDto>
{
    public UpdateEmployeeProfileValidator()
    {
        RuleFor(x => x.OtherName)
            .MaximumLength(100)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.OtherNameMaxLength);

        RuleFor(x => x.NRCNo)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.NRCMaxLength);

        RuleFor(x => x.Gender)
            .Must(g => string.IsNullOrEmpty(g) || g.Equals("Male", StringComparison.OrdinalIgnoreCase) || 
                      g.Equals("Female", StringComparison.OrdinalIgnoreCase) || g.Equals("Other", StringComparison.OrdinalIgnoreCase))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.GenderInvalid);

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.DateOfBirthFuture);

        RuleFor(x => x.Nationality)
            .MaximumLength(100)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.NationalityMaxLength);

        RuleFor(x => x.WorkPermitNo)
            .MaximumLength(50)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.WorkPermitNoMaxLength);

        RuleFor(x => x.WorkPermitValidDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.WorkPermitValidDate.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.WorkPermitValidDateFuture);

        RuleFor(x => x.WorkPermitExpireDate)
            .GreaterThan(x => x.WorkPermitValidDate)
            .When(x => x.WorkPermitValidDate.HasValue && x.WorkPermitExpireDate.HasValue)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.WorkPermitExpireDateAfterValid);

        RuleFor(x => x.ProfilePictureUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.ProfilePictureUrlInvalid);

        RuleFor(x => x.ProfileThumbnailUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.ProfileThumbnailUrlInvalid);

        RuleFor(x => x.AdditionalData)
            .MaximumLength(2000)
            .WithMessage(EmployeeInfoValidationMessages.EmployeeProfile.AdditionalDataMaxLength);
    }
}
