namespace EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs
{
    public class UpdateOneOnOneMeetingDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTimeOffset ScheduledDate { get; set; }
        public DateTimeOffset ScheduledEndTime { get; set; }
    }
}

public class CompleteMeetingDto
{
    public string? Summary { get; set; }
    public string? DiscussionNotes { get; set; }
    public string? PrivateNotes { get; set; }
    public string? ActionItems { get; set; }
}

public class RescheduleMeetingDto
{
    public DateTimeOffset ScheduledDate { get; set; }
    public DateTimeOffset ScheduledEndTime { get; set; }
}