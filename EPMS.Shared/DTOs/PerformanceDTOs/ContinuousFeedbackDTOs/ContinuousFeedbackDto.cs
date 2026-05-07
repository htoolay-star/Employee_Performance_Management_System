namespace EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs
{
    public class ContinuousFeedbackDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public long GivenById { get; set; }
        public string GivenByName { get; set; } = string.Empty;
        public long? RelatedGoalId { get; set; }
        public string FeedbackType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Visibility { get; set; } = string.Empty;
        public DateTimeOffset FeedbackDate { get; set; }
    }
}