using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class AppraisalCycle : IAuditableEntity
    {
        private AppraisalCycle() { }

        public AppraisalCycle(string name, int year, string type, DateOnly start, DateOnly end)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(type);

            if (start > end)
                throw new ArgumentException("Cycle StartDate cannot be after EndDate.");

            Name = name.Trim();
            AppraisalType = type.Trim().ToUpperInvariant();
            Year = year;
            StartDate = start;
            EndDate = end;

            IsActive = true;
            IsLocked = false;
        }

        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int Year { get; private set; }
        public string AppraisalType { get; private set; } = string.Empty;

        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }

        public DateOnly? SelfReviewDeadline { get; private set; }
        public DateOnly? ManagerReviewDeadline { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsLocked { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public void UpdatePeriod(DateOnly start, DateOnly end)
        {
            if (IsLocked)
                throw new InvalidOperationException("Cannot modify period dates of a locked appraisal cycle.");

            if (start > end)
                throw new ArgumentException("Cycle StartDate cannot be after EndDate.");

            StartDate = start;
            EndDate = end;
        }

        public void UpdateDeadlines(DateOnly? selfReview, DateOnly? managerReview)
        {
            if (IsLocked)
                throw new InvalidOperationException("Cannot modify deadlines of a locked appraisal cycle.");

            if (selfReview.HasValue && managerReview.HasValue && selfReview.Value > managerReview.Value)
                throw new ArgumentException("Self-review deadline cannot be after the manager review deadline.");

            SelfReviewDeadline = selfReview;
            ManagerReviewDeadline = managerReview;
        }

        public void LockCycle() => IsLocked = true;

        public void Deactivate() => IsActive = false;
    }
}
