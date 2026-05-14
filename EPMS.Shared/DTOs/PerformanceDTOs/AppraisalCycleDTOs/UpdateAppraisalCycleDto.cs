namespace EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;

public class UpdateAppraisalCycleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AppraisalType { get; set; } = string.Empty;
    public string CalendarType { get; set; } = string.Empty;
    public string YearLabel { get; set; } = string.Empty;
    public DateOnly EvaluationStartDate { get; set; }
    public DateOnly EvaluationEndDate { get; set; }
    public DateOnly WindowStartDate { get; set; }
    public DateOnly WindowEndDate { get; set; }
    public DateOnly? SelfReviewStartDate { get; set; }
    public DateOnly? SelfReviewDeadline { get; set; }
    public DateOnly? ManagerReviewStartDate { get; set; }
    public DateOnly? ManagerReviewDeadline { get; set; }
    public DateOnly? PeerReviewStartDate { get; set; }
    public DateOnly? PeerReviewDeadline { get; set; }
}
