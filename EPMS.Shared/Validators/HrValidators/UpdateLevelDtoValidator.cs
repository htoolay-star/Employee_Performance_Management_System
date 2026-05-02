using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.LevelDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.HrValidators;

public class UpdateLevelDtoValidator : AbstractValidator<UpdateLevelDto>
{
    public UpdateLevelDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(HrValidationMessages.Level.NameRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Level.NameMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage(HrValidationMessages.Level.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
