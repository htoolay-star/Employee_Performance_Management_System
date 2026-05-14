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

    public DateOnly? ManagerReviewStartDate { get; set; }
    public DateTime? ManagerReviewStartDateProxy
    {
        get => ManagerReviewStartDate?.ToDateTime(TimeOnly.MinValue);
        set => ManagerReviewStartDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? ManagerReviewDeadline { get; set; }
    public DateTime? ManagerReviewDeadlineProxy
    {
        get => ManagerReviewDeadline?.ToDateTime(TimeOnly.MinValue);
        set => ManagerReviewDeadline = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? PeerReviewStartDate { get; set; }
    public DateTime? PeerReviewStartDateProxy
    {
        get => PeerReviewStartDate?.ToDateTime(TimeOnly.MinValue);
        set => PeerReviewStartDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    public DateOnly? PeerReviewDeadline { get; set; }
    public DateTime? PeerReviewDeadlineProxy
    {
        get => PeerReviewDeadline?.ToDateTime(TimeOnly.MinValue);
        set => PeerReviewDeadline = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }
}
