using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using EPMS.Shared.Validators;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.AppraisalCycleDTOs;

public class CreateAppraisalCycleValidator : AbstractValidator<CreateAppraisalCycleDto>
{
    public CreateAppraisalCycleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.NameRequired)
            .MaximumLength(100).WithMessage(PerformanceValidationMessages.AppraisalCycle.NameMaxLength);

        RuleFor(x => x.CalendarType)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.CalendarTypeRequired)
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.AppraisalCycle.CalendarTypeMaxLength);

        RuleFor(x => x.YearLabel)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.YearLabelRequired)
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.AppraisalCycle.YearLabelMaxLength);

        RuleFor(x => x.AppraisalType)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.AppraisalTypeRequired)
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.AppraisalCycle.AppraisalTypeMaxLength);

        RuleFor(x => x.EvaluationStartDate)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.EvaluationStartDateRequired);

        RuleFor(x => x.EvaluationEndDate)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.EvaluationEndDateRequired)
            .GreaterThan(x => x.EvaluationStartDate)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.EvaluationEndAfterStart);

        RuleFor(x => x.WindowStartDate)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.WindowStartDateRequired);

        RuleFor(x => x.WindowEndDate)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.WindowEndDateRequired)
            .GreaterThan(x => x.WindowStartDate)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.WindowEndAfterStart);

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
