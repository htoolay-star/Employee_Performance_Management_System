namespace EPMS.Client.Models.Performance;

public class AppraisalCycleFormModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AppraisalType { get; set; } = string.Empty;
    public string CalendarType { get; set; } = string.Empty;
    public string YearLabel { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsLocked { get; set; }

    public DateOnly? EvaluationStartDate { get; set; }
    public DateTime? EvaluationStartDateProxy
    {
        get => EvaluationStartDate?.ToDateTime(TimeOnly.MinValue);
        set => EvaluationStartDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? EvaluationEndDate { get; set; }
    public DateTime? EvaluationEndDateProxy
    {
        get => EvaluationEndDate?.ToDateTime(TimeOnly.MinValue);
        set => EvaluationEndDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly WindowStartDate { get; set; }
    public DateTime? WindowStartDateProxy
    {
        get => WindowStartDate.ToDateTime(TimeOnly.MinValue);
        set => WindowStartDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : default;
    }

    public DateOnly WindowEndDate { get; set; }
    public DateTime? WindowEndDateProxy
    {
        get => WindowEndDate.ToDateTime(TimeOnly.MinValue);
        set => WindowEndDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : default;
    }

    public DateOnly? SelfReviewStartDate { get; set; }
    public DateTime? SelfReviewStartDateProxy
    {
        get => SelfReviewStartDate?.ToDateTime(TimeOnly.MinValue);
        set => SelfReviewStartDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? SelfReviewDeadline { get; set; }
    public DateTime? SelfReviewDeadlineProxy
    {
        get => SelfReviewDeadline?.ToDateTime(TimeOnly.MinValue);
        set => SelfReviewDeadline = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? AppraisalReviewStartDate { get; set; }
    public DateTime? AppraisalReviewStartDateProxy
    {
        get => AppraisalReviewStartDate?.ToDateTime(TimeOnly.MinValue);
        set => AppraisalReviewStartDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? AppraisalReviewDeadline { get; set; }
    public DateTime? AppraisalReviewDeadlineProxy
    {
        get => AppraisalReviewDeadline?.ToDateTime(TimeOnly.MinValue);
        set => AppraisalReviewDeadline = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? ThreeSixtyReviewStartDate { get; set; }
    public DateTime? ThreeSixtyReviewStartDateProxy
    {
        get => ThreeSixtyReviewStartDate?.ToDateTime(TimeOnly.MinValue);
        set => ThreeSixtyReviewStartDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? ThreeSixtyReviewDeadline { get; set; }
    public DateTime? ThreeSixtyReviewDeadlineProxy
    {
        get => ThreeSixtyReviewDeadline?.ToDateTime(TimeOnly.MinValue);
        set => ThreeSixtyReviewDeadline = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public decimal KpiWeight { get; set; } = 50m;
    public decimal SelfWeight { get; set; } = 15m;
    public decimal ThreeSixtyWeight { get; set; } = 10m;
    public decimal AppraisalWeight { get; set; } = 25m;
}
