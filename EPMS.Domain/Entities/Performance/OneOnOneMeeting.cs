using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class OneOnOneMeeting : AuditableEntity , ISoftDeletable
    {
        private OneOnOneMeeting() { }

        public OneOnOneMeeting(long employeeId, long managerId, string title, DateTimeOffset scheduledDate)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);

            EmployeeId = employeeId;
            ManagerId = managerId;

            Title = title.Trim();

            ScheduledDate = scheduledDate;
            Status = MeetingStatuses.Scheduled;
        }

        public long EmployeeId { get; private set; }
        public long ManagerId { get; private set; }

        public DateTimeOffset ScheduledDate { get; private set; }
        public DateTimeOffset? ActualDate { get; private set; }

        public string Title { get; private set; } = string.Empty;
        public string? Summary { get; private set; }
        public string? DiscussionNotes { get; private set; }
        public string? PrivateNotes { get; private set; }
        public string? ActionItems { get; private set; }

        public string Status { get; private set; } = string.Empty;

        public bool IsAcknowledgedByEmployee { get; private set; }
        public DateTimeOffset? AcknowledgedAt { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();
        public long? RelatedPIPId { get; private set; }
        public string MeetingType { get; private set; } = MeetingTypes.Regular;
        public virtual PIP? RelatedPIP { get; private set; }

        public virtual EmployeeProfile Employee { get; private set; } = null!;
        public virtual EmployeeProfile Manager { get; private set; } = null!;

        public void CompleteMeeting(string? summary, string? sharedNotes, string? privateNotes, string? actionItems, TimeProvider timeProvider)
        {
            Status = MeetingStatuses.Completed;
            ActualDate = timeProvider.GetUtcNow();

            Summary = summary?.Trim();
            DiscussionNotes = sharedNotes?.Trim();
            PrivateNotes = privateNotes?.Trim();
            ActionItems = actionItems?.Trim();
        }

        public void AcknowledgeByEmployee(TimeProvider timeProvider)
        {
            if (Status != MeetingStatuses.Completed) throw new InvalidOperationException("Only completed meetings can be acknowledged.");

            IsAcknowledgedByEmployee = true;
            AcknowledgedAt = timeProvider.GetUtcNow();
        }

        public void Cancel()
        {
            if (Status == MeetingStatuses.Completed) throw new InvalidOperationException("Cannot cancel a completed meeting.");
            if (Status == MeetingStatuses.Cancelled) throw new InvalidOperationException("Meeting is already cancelled.");
            Status = MeetingStatuses.Cancelled;
        }

        public void LinkToPIP(long pipId)
        {
            RelatedPIPId = pipId;
            MeetingType = MeetingTypes.PIPReview;
        }
    }
}
