using EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.KPIMasterDTOs
{
    public class UpdateKPIMasterValidator : AbstractValidator<UpdateKPIMasterDto>
    {
        public UpdateKPIMasterValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(PerformanceValidationMessages.KPIMaster.IdRequired);

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
        }
    }
}