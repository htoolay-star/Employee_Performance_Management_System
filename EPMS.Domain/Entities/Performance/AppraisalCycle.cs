using EPMS.Domain.Contracts;
using System;

namespace EPMS.Domain.Entities.Performance
{
    public class AppraisalCycle : AuditableEntity, ISoftDeletable
    {
        private AppraisalCycle() { }

        public AppraisalCycle(string name, string appraisalType, string calendarType, string yearLabel,
                              DateOnly evalStart, DateOnly evalEnd,
                              DateOnly windowStart, DateOnly windowEnd)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(appraisalType);
            ArgumentException.ThrowIfNullOrWhiteSpace(calendarType);
            ArgumentException.ThrowIfNullOrWhiteSpace(yearLabel);

            if (evalStart > evalEnd)
                throw new ArgumentException("Evaluation StartDate cannot be after EndDate.");

            if (windowStart > windowEnd)
                throw new ArgumentException("Appraisal Window StartDate cannot be after EndDate.");

            Name = name.Trim();
            AppraisalType = appraisalType.Trim().ToUpperInvariant();
            CalendarType = calendarType.Trim();
            YearLabel = yearLabel.Trim();

            EvaluationStartDate = evalStart;
            EvaluationEndDate = evalEnd;

            WindowStartDate = windowStart;
            WindowEndDate = windowEnd;

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

        public DateOnly? PeerReviewStartDate { get; private set; }
        public DateOnly? PeerReviewDeadline { get; private set; }

        public DateOnly? SelfReviewStartDate { get; private set; }
        public DateOnly? SelfReviewDeadline { get; private set; }

        public DateOnly? ManagerReviewStartDate { get; private set; }
        public DateOnly? ManagerReviewDeadline { get; private set; }

        public DateTimeOffset? FinalClosureDate { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsLocked { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

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

        public void ConfigureManagerReviewWindow(DateOnly start, DateOnly deadline)
        {
            if (start > deadline)
                throw new ArgumentException("Start date cannot be after the deadline.");

            if (start < WindowStartDate || deadline > WindowEndDate)
                throw new ArgumentException("The manager review window strictly must fall within the overall Appraisal Window dates.");

            ManagerReviewStartDate = start;
            ManagerReviewDeadline = deadline;
        }

        public void ConfigurePeerReviewWindow(DateOnly start, DateOnly deadline)
        {
            if (start > deadline)
                throw new ArgumentException("Start date cannot be after the deadline.");

            if (start < WindowStartDate || deadline > WindowEndDate)
                throw new ArgumentException("The peer review window strictly must fall within the overall Appraisal Window dates.");

            PeerReviewStartDate = start;
            PeerReviewDeadline = deadline;
        }

        public void Update(string name, DateOnly evalStart, DateOnly evalEnd,
                           DateOnly windowStart, DateOnly windowEnd)
        {
            if (IsLocked) throw new InvalidOperationException("Cannot update a locked cycle.");
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            if (evalStart > evalEnd)
                throw new ArgumentException("Evaluation start date cannot be after end date.");
            if (windowStart > windowEnd)
                throw new ArgumentException("Window start date cannot be after end date.");
            Name = name.Trim();
            EvaluationStartDate = evalStart;
            EvaluationEndDate = evalEnd;
            WindowStartDate = windowStart;
            WindowEndDate = windowEnd;
        }

        public void LockCycle() => IsLocked = true;

        public void Deactivate() => IsActive = false;
    }
}