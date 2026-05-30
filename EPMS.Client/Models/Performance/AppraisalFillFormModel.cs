namespace EPMS.Client.Models.Performance;

public class AppraisalFillFormModel
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string StaffNo { get; set; } = string.Empty;
    public string? PositionName { get; set; }
    public string? DepartmentName { get; set; }
    public string? TeamName { get; set; }
    public string? ManagerName { get; set; }
    public long CycleId { get; set; }
    public string? CycleName { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
    public bool KpiLocked { get; set; }
    public bool KpiLockIsDeadline { get; set; }
    public bool KpiUnlockRequested { get; set; }
    public string KpiStatus { get; set; } = string.Empty;
    public string SelfStatus { get; set; } = string.Empty;
    public string ManagerStatus { get; set; } = string.Empty;
    public string PeerStatus { get; set; } = string.Empty;
    public string SubordinateStatus { get; set; } = string.Empty;
    public string CommitteeStatus { get; set; } = string.Empty;
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
