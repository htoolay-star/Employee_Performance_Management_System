using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.LevelDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.HrValidators;

public class CreateLevelDtoValidator : AbstractValidator<CreateLevelDto>
{
    public CreateLevelDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(HrValidationMessages.Level.CodeRequired)
            .MaximumLength(10).WithMessage(HrValidationMessages.Level.CodeMaxLength);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(HrValidationMessages.Level.NameRequired)
            .MaximumLength(100).WithMessage(HrValidationMessages.Level.NameMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage(HrValidationMessages.Level.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
