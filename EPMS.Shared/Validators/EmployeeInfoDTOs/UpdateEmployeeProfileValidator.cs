using EPMS.Shared.DTOs.EmployeeInfoDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.EmployeeInfoDTOs;

public class UpdateEmployeeProfileValidator : AbstractValidator<UpdateEmployeeProfileDto>
{
    public UpdateEmployeeProfileValidator()
    {
        RuleFor(x => x.OtherName)
            .MaximumLength(100)
            .WithMessage("Other name cannot exceed 100 characters.");

        RuleFor(x => x.NRCNo)
            .MaximumLength(50)
            .WithMessage("NRC number cannot exceed 50 characters.");

        RuleFor(x => x.Gender)
            .Must(g => string.IsNullOrEmpty(g) || g.Equals("Male", StringComparison.OrdinalIgnoreCase) || 
                      g.Equals("Female", StringComparison.OrdinalIgnoreCase) || g.Equals("Other", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Gender must be Male, Female, or Other.");

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth cannot be in the future.");

        RuleFor(x => x.Nationality)
            .MaximumLength(100)
            .WithMessage("Nationality cannot exceed 100 characters.");

        RuleFor(x => x.WorkPermitNo)
            .MaximumLength(50)
            .WithMessage("Work permit number cannot exceed 50 characters.");

        RuleFor(x => x.WorkPermitValidDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.WorkPermitValidDate.HasValue)
            .WithMessage("Work permit valid date cannot be in the future.");

        RuleFor(x => x.WorkPermitExpireDate)
            .GreaterThan(x => x.WorkPermitValidDate)
            .When(x => x.WorkPermitValidDate.HasValue && x.WorkPermitExpireDate.HasValue)
            .WithMessage("Work permit expire date must be after valid date.");

        RuleFor(x => x.ProfilePictureUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Profile picture URL must be a valid URL.");

        RuleFor(x => x.ProfileThumbnailUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Profile thumbnail URL must be a valid URL.");

        RuleFor(x => x.AdditionalData)
            .MaximumLength(2000)
            .WithMessage("Additional data cannot exceed 2000 characters.");
    }
}
