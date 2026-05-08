namespace EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs
{
    public class OneOnOneMeetingDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public long ManagerId { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTimeOffset ScheduledDate { get; set; }
        public DateTimeOffset? ActualDate { get; set; }
        public string? Summary { get; set; }
        public string? DiscussionNotes { get; set; }
        public string? PrivateNotes { get; set; }
        public string? ActionItems { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsAcknowledgedByEmployee { get; set; }
        public DateTimeOffset? AcknowledgedAt { get; set; }
        public long? RelatedPIPId { get; set; }
        public string MeetingType { get; set; } = string.Empty;
    }
}