using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.KPIMasterDTOs
{
    public class CreateKPIMasterValidator : AbstractValidator<CreateKPIMasterDto>
    {
        public CreateKPIMasterValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.KPIMaster.CategoryIdRequired);

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(PerformanceValidationMessages.KPIMaster.CodeRequired)
                .MaximumLength(20).WithMessage(PerformanceValidationMessages.KPIMaster.CodeMaxLength);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(PerformanceValidationMessages.KPIMaster.NameRequired)
                .MaximumLength(100).WithMessage(PerformanceValidationMessages.KPIMaster.NameMaxLength);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(PerformanceValidationMessages.KPIMaster.DescriptionMaxLength);

            RuleFor(x => x.ScoringDirection)
                .Must(d => AppraisalConstants.ScoringDirections.All.Contains(d))
                .WithMessage(PerformanceValidationMessages.KPIMaster.ScoringDirectionInvalid);
        }
    }
}