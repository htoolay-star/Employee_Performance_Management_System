namespace EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;

public class CreateAppraisalCycleDto
{
    public string Name { get; set; } = string.Empty;
    public string CalendarType { get; set; } = string.Empty;
    public string YearLabel { get; set; } = string.Empty;
    public string AppraisalType { get; set; } = string.Empty;
    public DateOnly? EvaluationStartDate { get; set; }
    public DateOnly? EvaluationEndDate { get; set; }
    public DateOnly WindowStartDate { get; set; }
    public DateOnly WindowEndDate { get; set; }
    public DateOnly? SelfReviewStartDate { get; set; }
    public DateOnly? SelfReviewDeadline { get; set; }
    public DateOnly? ManagerReviewStartDate { get; set; }
    public DateOnly? ManagerReviewDeadline { get; set; }
    public DateOnly? ThreeSixtyReviewStartDate { get; set; }
    public DateOnly? ThreeSixtyReviewDeadline { get; set; }

    public decimal KpiWeight { get; set; } = 50m;
    public decimal SelfWeight { get; set; } = 15m;
    public decimal ThreeSixtyWeight { get; set; } = 10m;
    public decimal AppraisalWeight { get; set; } = 25m;
    public long? AppraisalReviewerId { get; set; }
}
