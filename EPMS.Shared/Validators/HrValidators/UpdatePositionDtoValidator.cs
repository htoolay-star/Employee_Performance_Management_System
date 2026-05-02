using EPMS.Shared.Constants.ValidationMessages;
using EPMS.Shared.DTOs.PositionDTOs;
using FluentValidation;

namespace EPMS.Shared.Validators.HrValidators
{
    public class UpdatePositionDtoValidator : AbstractValidator<UpdatePositionDto>
    {
        public UpdatePositionDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(HrValidationMessages.Position.TitleRequired)
                .MaximumLength(100).WithMessage(HrValidationMessages.Position.TitleMaxLength);

            RuleFor(x => x.LevelId)
                .GreaterThan(0).WithMessage(HrValidationMessages.Position.LevelIdInvalid);
        }
    }
}
