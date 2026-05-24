using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Shared.Constants;
using System;

namespace EPMS.Domain.Entities.Performance
{
    public class AppraisalCycle : AuditableEntity, ISoftDeletable
    {
        private AppraisalCycle() { }

        public AppraisalCycle(string name, string appraisalType, string calendarType, string yearLabel,
                              DateOnly evalStart, DateOnly evalEnd,
                              DateOnly windowStart, DateOnly windowEnd,
                              decimal kpiWeight = 50m, decimal selfWeight = 15m,
                              decimal peerWeight = 10m,
                              decimal appraisalWeight = 25m)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(appraisalType);
            ArgumentException.ThrowIfNullOrWhiteSpace(calendarType);
            ArgumentException.ThrowIfNullOrWhiteSpace(yearLabel);

            if (evalStart > evalEnd)
                throw new ArgumentException("Evaluation StartDate cannot be after EndDate.");

            if (windowStart < evalEnd)
                throw new ArgumentException("Window start date cannot be before evaluation end date.");

            if (windowStart > windowEnd)
                throw new ArgumentException("Appraisal Window StartDate cannot be after EndDate.");

            ValidateEvaluationDuration(appraisalType, evalStart, evalEnd);
            ValidateEvaluationDatesWithinYear(calendarType, yearLabel, evalStart, evalEnd);

            Name = name.Trim();
            AppraisalType = appraisalType.Trim().ToUpperInvariant();
            CalendarType = calendarType.Trim().ToUpperInvariant();
            YearLabel = yearLabel.Trim();

            EvaluationStartDate = evalStart;
            EvaluationEndDate = evalEnd;

            WindowStartDate = windowStart;
            WindowEndDate = windowEnd;

            SetWeights(kpiWeight, selfWeight, peerWeight, appraisalWeight);

            IsActive = true;
            IsLocked = false;
        }

        public string Name { get; private set; } = string.Empty;
        public string AppraisalType { get; private set; } = string.Empty;

        public string CalendarType { get; private set; } = string.Empty;
        public string YearLabel { get; private set; } = string.Empty;

        public DateOnly EvaluationStartDate { get; private set; }
        public DateOnly EvaluationEndDate { get; private set; }

        public DateOnly WindowStartDate { get; private set; }
        public DateOnly WindowEndDate { get; private set; }

        public DateOnly? ThreeSixtyReviewStartDate { get; private set; }
        public DateOnly? ThreeSixtyReviewDeadline { get; private set; }

        public DateOnly? SelfReviewStartDate { get; private set; }
        public DateOnly? SelfReviewDeadline { get; private set; }

        public DateOnly? ManagerReviewStartDate { get; private set; }
        public DateOnly? ManagerReviewDeadline { get; private set; }

        public DateTimeOffset? FinalClosureDate { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsLocked { get; private set; }

        public decimal KpiWeight { get; private set; }
        public decimal SelfWeight { get; private set; }
        public decimal ThreeSixtyWeight { get; private set; }
        public decimal AppraisalWeight { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        private readonly List<EmployeeKPI> _employeeKPIs = new();
        public virtual IReadOnlyCollection<EmployeeKPI> EmployeeKPIs => _employeeKPIs.AsReadOnly();

        public void ConfigureSelfReviewWindow(DateOnly start, DateOnly deadline)
        {
            if (start > deadline)
                throw new ArgumentException("Start date cannot be after the deadline.");

            if (start < WindowStartDate || deadline > WindowEndDate)
                throw new ArgumentException("The self-review window strictly must fall within the overall Appraisal Window dates.");

            SelfReviewStartDate = start;
            SelfReviewDeadline = deadline;
        }

        public void ConfigureAppraisalReviewWindow(DateOnly start, DateOnly deadline)
        {
            if (start > deadline)
                throw new ArgumentException("Start date cannot be after the deadline.");

            if (start < WindowStartDate || deadline > WindowEndDate)
                throw new ArgumentException("The appraisal review window strictly must fall within the overall Appraisal Window dates.");

            ManagerReviewStartDate = start;
            ManagerReviewDeadline = deadline;
        }

        public void ConfigureThreeSixtyReviewWindow(DateOnly start, DateOnly deadline)
        {
            if (start > deadline)
                throw new ArgumentException("Start date cannot be after the deadline.");

            if (start < WindowStartDate || deadline > WindowEndDate)
                throw new ArgumentException("The peer review window strictly must fall within the overall Appraisal Window dates.");

            ThreeSixtyReviewStartDate = start;
            ThreeSixtyReviewDeadline = deadline;
        }

        public void Update(string name, string appraisalType, string calendarType, string yearLabel,
                           DateOnly evalStart, DateOnly evalEnd,
                           DateOnly windowStart, DateOnly windowEnd,
                           decimal? kpiWeight = null, decimal? selfWeight = null,
                           decimal? peerWeight = null,
                           decimal? appraisalWeight = null)
        {
            if (IsLocked) throw new InvalidOperationException("Cannot update a locked cycle.");
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(appraisalType);
            ArgumentException.ThrowIfNullOrWhiteSpace(calendarType);
            ArgumentException.ThrowIfNullOrWhiteSpace(yearLabel);
            if (evalStart > evalEnd)
                throw new ArgumentException("Evaluation start date cannot be after end date.");
            if (windowStart < evalEnd)
                throw new ArgumentException("Window start date cannot be before evaluation end date.");
            if (windowStart > windowEnd)
                throw new ArgumentException("Window start date cannot be after end date.");
            ValidateEvaluationDuration(appraisalType, evalStart, evalEnd);
            ValidateEvaluationDatesWithinYear(calendarType, yearLabel, evalStart, evalEnd);
            Name = name.Trim();
            AppraisalType = appraisalType.Trim().ToUpperInvariant();
            CalendarType = calendarType.Trim().ToUpperInvariant();
            YearLabel = yearLabel.Trim();
            EvaluationStartDate = evalStart;
            EvaluationEndDate = evalEnd;
            WindowStartDate = windowStart;
            WindowEndDate = windowEnd;

            if (kpiWeight.HasValue || selfWeight.HasValue || peerWeight.HasValue || appraisalWeight.HasValue)
                SetWeights(
                    kpiWeight ?? KpiWeight,
                    selfWeight ?? SelfWeight,
                    peerWeight ?? ThreeSixtyWeight,
                    appraisalWeight ?? AppraisalWeight);
        }

        public void SetWeights(decimal kpiWeight, decimal selfWeight, decimal peerWeight, decimal appraisalWeight = 0m)
        {
            var total = kpiWeight + selfWeight + peerWeight + appraisalWeight;
            if (total != 100m)
                throw new ArgumentException($"Weights must sum to 100. Current total: {total}.");

            if (kpiWeight < 0 || selfWeight < 0 || peerWeight < 0 || appraisalWeight < 0)
                throw new ArgumentException("Individual weights cannot be negative.");

            KpiWeight = kpiWeight;
            SelfWeight = selfWeight;
            ThreeSixtyWeight = peerWeight;
            AppraisalWeight = appraisalWeight;
        }

        public void LockCycle()
        {
            IsLocked = true;
            IsActive = false;
        }

        public void Deactivate() => IsActive = false;

        public void Reactivate() => IsActive = true;

        private static void ValidateEvaluationDatesWithinYear(string calendarType, string yearLabel, DateOnly start, DateOnly end)
        {
            var upperCal = calendarType.Trim().ToUpperInvariant();
            var label = yearLabel.Trim();

            if (!int.TryParse(label[..Math.Min(4, label.Length)], out var year))
                throw new ArgumentException($"Invalid year label: '{label}'.");

            DateOnly yearStart, yearEnd;
            if (upperCal == AppraisalConstants.CalendarTypes.FiscalYear)
            {
                yearStart = new DateOnly(year, 4, 1);
                yearEnd = new DateOnly(year + 1, 3, 31);
            }
            else
            {
                yearStart = new DateOnly(year, 1, 1);
                yearEnd = new DateOnly(year, 12, 31);
            }

            if (start < yearStart || end > yearEnd)
                throw new ArgumentException(
                    $"Evaluation dates must fall within the allowed range for '{yearLabel}': {yearStart:dd/MM/yyyy} to {yearEnd:dd/MM/yyyy}.");
        }

        private static void ValidateEvaluationDuration(string appraisalType, DateOnly start, DateOnly end)
        {
            var upper = appraisalType.Trim().ToUpperInvariant();
            var (minDays, maxDays) = upper switch
            {
                AppraisalConstants.AppraisalTypes.Monthly => (20, 31),
                AppraisalConstants.AppraisalTypes.Quarterly => (60, 92),
                AppraisalConstants.AppraisalTypes.SemiAnnual => (120, 184),
                AppraisalConstants.AppraisalTypes.Annual => (300, 366),
                _ => (300, 366)
            };
            var actualDays = end.DayNumber - start.DayNumber;
            if (actualDays < minDays)
                throw new ArgumentException(
                    $"Evaluation period ({actualDays} days) is below minimum of {minDays} days for {upper}.");
            if (actualDays > maxDays)
                throw new ArgumentException(
                    $"Evaluation period ({actualDays} days) exceeds maximum of {maxDays} days for {upper}.");
        }
    }
}