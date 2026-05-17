using EPMS.Shared.Constants;
using EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;
using EPMS.Shared.Validators;
using EPMS.Shared.Validators.ValidationMessages;
using FluentValidation;

namespace EPMS.Shared.Validators.PerformanceDTOs.AppraisalCycleDTOs;

public class UpdateAppraisalCycleValidator : AbstractValidator<UpdateAppraisalCycleDto>
{
    public UpdateAppraisalCycleValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.IdRequired);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.NameRequired)
            .MaximumLength(100).WithMessage(PerformanceValidationMessages.AppraisalCycle.NameMaxLength);

        RuleFor(x => x.CalendarType)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.CalendarTypeRequired)
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.AppraisalCycle.CalendarTypeMaxLength)
            .Must(x => string.IsNullOrEmpty(x) || AppraisalConstants.CalendarTypes.All.Contains(x.Trim().ToUpperInvariant()))
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.CalendarTypeInvalid);

        RuleFor(x => x.YearLabel)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.YearLabelRequired)
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.AppraisalCycle.YearLabelMaxLength);

        RuleFor(x => x.AppraisalType)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.AppraisalTypeRequired)
            .MaximumLength(50).WithMessage(PerformanceValidationMessages.AppraisalCycle.AppraisalTypeMaxLength)
            .Must(x => string.IsNullOrEmpty(x) || AppraisalConstants.AppraisalTypes.All.Contains(x.Trim().ToUpperInvariant()))
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.AppraisalTypeInvalid);

        RuleFor(x => x.EvaluationStartDate)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.EvaluationStartDateRequired);

        RuleFor(x => x.EvaluationStartDate)
            .Must((dto, startDate) =>
            {
                if (string.IsNullOrEmpty(dto.CalendarType) || string.IsNullOrEmpty(dto.YearLabel))
                    return true;
                var upperCal = dto.CalendarType.Trim().ToUpperInvariant();
                if (!int.TryParse(dto.YearLabel[..Math.Min(4, dto.YearLabel.Length)], out var year))
                    return true;
                var yearStart = upperCal == AppraisalConstants.CalendarTypes.FiscalYear
                    ? new DateOnly(year, 4, 1)
                    : new DateOnly(year, 1, 1);
                var yearEnd = upperCal == AppraisalConstants.CalendarTypes.FiscalYear
                    ? new DateOnly(year + 1, 3, 31)
                    : new DateOnly(year, 12, 31);
                return startDate >= yearStart && startDate <= yearEnd;
            })
            .When(x => !string.IsNullOrEmpty(x.CalendarType) && !string.IsNullOrEmpty(x.YearLabel))
            .WithMessage(x =>
            {
                var upperCal = x.CalendarType.Trim().ToUpperInvariant();
                _ = int.TryParse(x.YearLabel[..Math.Min(4, x.YearLabel.Length)], out var year);
                var yearStart = upperCal == AppraisalConstants.CalendarTypes.FiscalYear
                    ? new DateOnly(year, 4, 1)
                    : new DateOnly(year, 1, 1);
                var yearEnd = upperCal == AppraisalConstants.CalendarTypes.FiscalYear
                    ? new DateOnly(year + 1, 3, 31)
                    : new DateOnly(year, 12, 31);
                return string.Format(PerformanceValidationMessages.AppraisalCycle.EvaluationDateOutsideYearRange,
                    yearStart, yearEnd, x.YearLabel);
            });

        RuleFor(x => x.EvaluationEndDate)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.EvaluationEndDateRequired)
            .GreaterThan(x => x.EvaluationStartDate)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.EvaluationEndAfterStart)
            .Must((dto, endDate) =>
            {
                if (string.IsNullOrEmpty(dto.AppraisalType))
                    return true;
                var upperType = dto.AppraisalType.Trim().ToUpperInvariant();
                var (minDays, maxDays) = upperType switch
                {
                    AppraisalConstants.AppraisalTypes.Monthly => (20, 31),
                    AppraisalConstants.AppraisalTypes.Quarterly => (60, 92),
                    AppraisalConstants.AppraisalTypes.SemiAnnual => (120, 184),
                    AppraisalConstants.AppraisalTypes.Annual => (300, 366),
                    _ => (300, 366)
                };
                var actualDays = endDate.DayNumber - dto.EvaluationStartDate.DayNumber;
                return actualDays >= minDays && actualDays <= maxDays;
            })
            .When(x => !string.IsNullOrEmpty(x.AppraisalType))
            .WithMessage(x =>
            {
                var upperType = x.AppraisalType.Trim().ToUpperInvariant();
                var (minDays, maxDays) = upperType switch
                {
                    AppraisalConstants.AppraisalTypes.Monthly => (20, 31),
                    AppraisalConstants.AppraisalTypes.Quarterly => (60, 92),
                    AppraisalConstants.AppraisalTypes.SemiAnnual => (120, 184),
                    AppraisalConstants.AppraisalTypes.Annual => (300, 366),
                    _ => (300, 366)
                };
                var actualDays = x.EvaluationEndDate.DayNumber - x.EvaluationStartDate.DayNumber;
                if (actualDays < minDays)
                    return string.Format(PerformanceValidationMessages.AppraisalCycle.EvaluationPeriodBelowMinimum,
                        actualDays, minDays, upperType);
                return string.Format(PerformanceValidationMessages.AppraisalCycle.EvaluationPeriodExceedsMax,
                    actualDays, maxDays, upperType);
            })
            .Must((dto, endDate) =>
            {
                if (string.IsNullOrEmpty(dto.CalendarType) || string.IsNullOrEmpty(dto.YearLabel))
                    return true;
                var upperCal = dto.CalendarType.Trim().ToUpperInvariant();
                if (!int.TryParse(dto.YearLabel[..Math.Min(4, dto.YearLabel.Length)], out var year))
                    return true;
                var yearStart = upperCal == AppraisalConstants.CalendarTypes.FiscalYear
                    ? new DateOnly(year, 4, 1)
                    : new DateOnly(year, 1, 1);
                var yearEnd = upperCal == AppraisalConstants.CalendarTypes.FiscalYear
                    ? new DateOnly(year + 1, 3, 31)
                    : new DateOnly(year, 12, 31);
                return endDate <= yearEnd && endDate >= yearStart;
            })
            .When(x => !string.IsNullOrEmpty(x.CalendarType) && !string.IsNullOrEmpty(x.YearLabel))
            .WithMessage(x =>
            {
                var upperCal = x.CalendarType.Trim().ToUpperInvariant();
                _ = int.TryParse(x.YearLabel[..Math.Min(4, x.YearLabel.Length)], out var year);
                var yearStart = upperCal == AppraisalConstants.CalendarTypes.FiscalYear
                    ? new DateOnly(year, 4, 1)
                    : new DateOnly(year, 1, 1);
                var yearEnd = upperCal == AppraisalConstants.CalendarTypes.FiscalYear
                    ? new DateOnly(year + 1, 3, 31)
                    : new DateOnly(year, 12, 31);
                return string.Format(PerformanceValidationMessages.AppraisalCycle.EvaluationDateOutsideYearRange,
                    yearStart, yearEnd, x.YearLabel);
            });

        RuleFor(x => x.WindowStartDate)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.WindowStartDateRequired)
            .GreaterThanOrEqualTo(x => x.EvaluationEndDate)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.WindowStartAfterEvaluationEnd);

        RuleFor(x => x.WindowEndDate)
            .NotEmpty().WithMessage(PerformanceValidationMessages.AppraisalCycle.WindowEndDateRequired)
            .GreaterThan(x => x.WindowStartDate)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.WindowEndAfterStart);

        RuleFor(x => x.SelfReviewStartDate)
            .LessThanOrEqualTo(x => x.SelfReviewDeadline)
            .When(x => x.SelfReviewStartDate.HasValue && x.SelfReviewDeadline.HasValue)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.SelfReviewStartBeforeDeadline);

        RuleFor(x => x.SelfReviewStartDate)
            .GreaterThanOrEqualTo(x => x.WindowStartDate)
            .When(x => x.SelfReviewStartDate.HasValue)
            .WithMessage(x => string.Format(
                PerformanceValidationMessages.AppraisalCycle.SelfReviewOutsideWindow,
                x.WindowStartDate, x.WindowEndDate));

        RuleFor(x => x.SelfReviewDeadline)
            .LessThanOrEqualTo(x => x.WindowEndDate)
            .When(x => x.SelfReviewDeadline.HasValue)
            .WithMessage(x => string.Format(
                PerformanceValidationMessages.AppraisalCycle.SelfReviewOutsideWindow,
                x.WindowStartDate, x.WindowEndDate));

        RuleFor(x => x.ManagerReviewStartDate)
            .LessThanOrEqualTo(x => x.ManagerReviewDeadline)
            .When(x => x.ManagerReviewStartDate.HasValue && x.ManagerReviewDeadline.HasValue)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.ManagerReviewStartBeforeDeadline);

        RuleFor(x => x.ManagerReviewStartDate)
            .GreaterThanOrEqualTo(x => x.WindowStartDate)
            .When(x => x.ManagerReviewStartDate.HasValue)
            .WithMessage(x => string.Format(
                PerformanceValidationMessages.AppraisalCycle.ManagerReviewOutsideWindow,
                x.WindowStartDate, x.WindowEndDate));

        RuleFor(x => x.ManagerReviewDeadline)
            .LessThanOrEqualTo(x => x.WindowEndDate)
            .When(x => x.ManagerReviewDeadline.HasValue)
            .WithMessage(x => string.Format(
                PerformanceValidationMessages.AppraisalCycle.ManagerReviewOutsideWindow,
                x.WindowStartDate, x.WindowEndDate));

        RuleFor(x => x.PeerReviewStartDate)
            .LessThanOrEqualTo(x => x.PeerReviewDeadline)
            .When(x => x.PeerReviewStartDate.HasValue && x.PeerReviewDeadline.HasValue)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.PeerReviewStartBeforeDeadline);

        RuleFor(x => x.PeerReviewStartDate)
            .GreaterThanOrEqualTo(x => x.WindowStartDate)
            .When(x => x.PeerReviewStartDate.HasValue)
            .WithMessage(x => string.Format(
                PerformanceValidationMessages.AppraisalCycle.PeerReviewOutsideWindow,
                x.WindowStartDate, x.WindowEndDate));

        RuleFor(x => x.PeerReviewDeadline)
            .LessThanOrEqualTo(x => x.WindowEndDate)
            .When(x => x.PeerReviewDeadline.HasValue)
            .WithMessage(x => string.Format(
                PerformanceValidationMessages.AppraisalCycle.PeerReviewOutsideWindow,
                x.WindowStartDate, x.WindowEndDate));

        RuleFor(x => x.KpiWeight)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.WeightCannotBeNegative);

        RuleFor(x => x.SelfWeight)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.WeightCannotBeNegative);

        RuleFor(x => x.PeerWeight)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.WeightCannotBeNegative);

        RuleFor(x => x.ManagerWeight)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PerformanceValidationMessages.AppraisalCycle.WeightCannotBeNegative);

        RuleFor(x => x)
            .Must(x => x.KpiWeight + x.SelfWeight + x.PeerWeight + x.ManagerWeight == 100m)
            .WithMessage(x => string.Format(
                PerformanceValidationMessages.AppraisalCycle.WeightsMustSumTo100,
                x.KpiWeight, x.SelfWeight, x.PeerWeight, x.ManagerWeight));
    }
}
