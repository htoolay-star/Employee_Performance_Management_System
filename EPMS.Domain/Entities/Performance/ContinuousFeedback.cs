using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class ContinuousFeedback : IAuditableEntity , ISoftDeletable
    {
        private ContinuousFeedback() { }

        public ContinuousFeedback(long employeeId, long givenById, string feedbackType, string content, string visibility = "Public", long? relatedGoalId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(feedbackType);
            ArgumentException.ThrowIfNullOrWhiteSpace(content);
            ArgumentException.ThrowIfNullOrWhiteSpace(visibility);

            EmployeeId = employeeId;
            GivenById = givenById;

            FeedbackType = feedbackType.Trim().ToUpperInvariant();
            Content = content.Trim();
            Visibility = visibility.Trim().ToUpperInvariant();

            RelatedGoalId = relatedGoalId;
            FeedbackDate = DateTimeOffset.UtcNow;
        }

        public long Id { get; private set; }
        public long EmployeeId { get; private set; }
        public long GivenById { get; private set; }
        public long? RelatedGoalId { get; private set; }

        public string FeedbackType { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public string Visibility { get; private set; } = string.Empty;

        public DateTimeOffset FeedbackDate { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        // Navigations
        public virtual EmployeeProfile Employee { get; private set; } = null!;
        public virtual EmployeeProfile GivenBy { get; private set; } = null!;
        public virtual KPIMaster? RelatedGoal { get; private set; }
    }
}
