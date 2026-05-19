namespace EPMS.Shared.DTOs.PerformanceDTOs.AppraisalCycleDTOs;

public class AppraisalCycleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CalendarType { get; set; } = string.Empty;
    public string YearLabel { get; set; } = string.Empty;
    public string AppraisalType { get; set; } = string.Empty;
    public DateOnly EvaluationStartDate { get; set; }
    public DateOnly EvaluationEndDate { get; set; }
    public DateOnly WindowStartDate { get; set; }
    public DateOnly WindowEndDate { get; set; }
    public DateOnly? SelfReviewStartDate { get; set; }
    public DateOnly? SelfReviewDeadline { get; set; }
    public DateOnly? AppraisalReviewStartDate { get; set; }
    public DateOnly? AppraisalReviewDeadline { get; set; }
    public DateOnly? ThreeSixtyReviewStartDate { get; set; }
    public DateOnly? ThreeSixtyReviewDeadline { get; set; }
    public DateTimeOffset? FinalClosureDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public decimal KpiWeight { get; set; }
    public decimal SelfWeight { get; set; }
    public decimal ThreeSixtyWeight { get; set; }
    public decimal AppraisalWeight { get; set; }
}
