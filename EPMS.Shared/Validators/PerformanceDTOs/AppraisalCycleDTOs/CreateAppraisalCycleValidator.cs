using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using EPMS.Shared.Validators;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.AppraisalCycleDTOs
{
    public class CreateAppraisalCycleValidator : AbstractValidator<CreateAppraisalCycleDto>
    {
        public CreateAppraisalCycleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.NameRequired)
                .MaximumLength(100).WithMessage(PerformanceValidationMessages.AppraisalCycle.NameMaxLength);

            RuleFor(x => x.Year)
                .InclusiveBetween(2000, 2100)
                .WithMessage(PerformanceValidationMessages.AppraisalCycle.YearInvalid);

            RuleFor(x => x.AppraisalType)
                .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.AppraisalTypeRequired)
                .MaximumLength(50).WithMessage(PerformanceValidationMessages.AppraisalCycle.AppraisalTypeMaxLength);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.StartDateRequired);

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.EndDateRequired)
                .GreaterThan(x => x.StartDate)
                .WithMessage(PerformanceValidationMessages.AppraisalCycle.EndDateAfterStart);

            RuleFor(x => x.SelfReviewStartDate)
                .LessThanOrEqualTo(x => x.SelfReviewDeadline)
                .When(x => x.SelfReviewStartDate.HasValue && x.SelfReviewDeadline.HasValue)
                .WithMessage(PerformanceValidationMessages.AppraisalCycle.SelfReviewStartBeforeDeadline);

            RuleFor(x => x.ManagerReviewStartDate)
                .LessThanOrEqualTo(x => x.ManagerReviewDeadline)
                .When(x => x.ManagerReviewStartDate.HasValue && x.ManagerReviewDeadline.HasValue)
                .WithMessage(PerformanceValidationMessages.AppraisalCycle.ManagerReviewStartBeforeDeadline);

            RuleFor(x => x.PeerReviewStartDate)
                .LessThanOrEqualTo(x => x.PeerReviewDeadline)
                .When(x => x.PeerReviewStartDate.HasValue && x.PeerReviewDeadline.HasValue)
                .WithMessage(PerformanceValidationMessages.AppraisalCycle.PeerReviewStartBeforeDeadline);
        }
    }
}