using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Shared.Constants;

namespace EPMS.Domain.Entities.Performance
{
    public class ContinuousFeedback : AuditableEntity, ISoftDeletable
    {
        private ContinuousFeedback() { }

        public ContinuousFeedback(long employeeId, long givenById, string feedbackType, string content, TimeProvider timeProvider, string visibility = FeedbackVisibility.Public, long? relatedGoalId = null)
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
            FeedbackDate = timeProvider.GetUtcNow();
        }

        public long EmployeeId { get; private set; }
        public long GivenById { get; private set; }
        public long? RelatedGoalId { get; private set; }

        public string FeedbackType { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public string Visibility { get; private set; } = string.Empty;

        public DateTimeOffset FeedbackDate { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual EmployeeProfile Employee { get; private set; } = null!;
        public virtual EmployeeProfile GivenBy { get; private set; } = null!;
        public virtual KPIMaster? RelatedGoal { get; private set; }

        public void Update(string content, string visibility)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(content);
            ArgumentException.ThrowIfNullOrWhiteSpace(visibility);

            Content = content.Trim();
            Visibility = visibility.Trim().ToUpperInvariant();
        }
    }
}
