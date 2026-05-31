namespace EPMS.Shared.DTOs.PerformanceDTOs.OneOnOneMeetingDTOs
{
    public class CreateOneOnOneMeetingDto
    {
        public long EmployeeId { get; set; }
        public long ManagerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTimeOffset ScheduledDate { get; set; }
        public DateTimeOffset ScheduledEndTime { get; set; }
        public long? RelatedPIPId { get; set; }
    }
}