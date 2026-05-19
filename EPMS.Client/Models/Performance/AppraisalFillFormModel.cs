namespace EPMS.Client.Models.Performance;

public class AppraisalFillFormModel
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public long CycleId { get; set; }
    public string? CycleName { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
    public List<AppraisalDetailFillItem> Details { get; set; } = new();
}

public class AppraisalDetailFillItem
{
    public long? KPIId { get; set; }
    public string KPIName { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public decimal Weightage { get; set; }
    public decimal? TargetValue { get; set; }
    public string ScoringDirection { get; set; } = string.Empty;
    public decimal? ActualValue { get; set; }
    public decimal Score { get; set; }
    public decimal WeightedScore { get; set; }
    public string? Remarks { get; set; }
}
