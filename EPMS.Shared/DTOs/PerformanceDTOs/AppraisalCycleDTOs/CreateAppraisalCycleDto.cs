namespace EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs
{
    public class CreateAppraisalCycleDto
    {
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }
        public string AppraisalType { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly? PeerReviewStartDate { get; set; }
        public DateOnly? PeerReviewDeadline { get; set; }
        public DateOnly? SelfReviewStartDate { get; set; }
        public DateOnly? SelfReviewDeadline { get; set; }
        public DateOnly? ManagerReviewStartDate { get; set; }
        public DateOnly? ManagerReviewDeadline { get; set; }
    }
}