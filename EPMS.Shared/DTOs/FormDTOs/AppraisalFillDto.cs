namespace EPMS.Shared.DTOs.FormDTOs;

public class AppraisalFillDto
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
    public long ManagerReviewerId { get; set; }
    public string? ManagerReviewerName { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
    public bool KpiLocked { get; set; }
    public string KpiStatus { get; set; } = string.Empty;
    public string SelfStatus { get; set; } = string.Empty;
    public string ManagerStatus { get; set; } = string.Empty;
    public string PeerStatus { get; set; } = string.Empty;
    public string SubordinateStatus { get; set; } = string.Empty;
    public string CommitteeStatus { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public long? EntityId { get; set; }
    public string? EntityName { get; set; }
    public string? EntityHeadName { get; set; }
    public List<AppraisalDetailFillDto> Details { get; set; } = new();
}
