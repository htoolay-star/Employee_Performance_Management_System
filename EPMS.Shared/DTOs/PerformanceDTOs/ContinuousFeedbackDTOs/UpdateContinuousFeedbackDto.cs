namespace EPMS.Shared.DTOs.PerformanceDTOs.ContinuousFeedbackDTOs
{
    public class UpdateContinuousFeedbackDto
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Visibility { get; set; } = string.Empty;
    }
}