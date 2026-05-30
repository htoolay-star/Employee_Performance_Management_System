namespace EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs
{
    public class CreateContinuousFeedbackDto
    {
        public long EmployeeId { get; set; }
        public long? RelatedGoalId { get; set; }
        public string FeedbackType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Visibility { get; set; } = "PUBLIC";
    }
}