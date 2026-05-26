namespace EPMS.Shared.DTOs.FormDTOs;

public class MyEvaluationFormDto
{
    public long AppraisalId { get; set; }
    public long EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? PositionName { get; set; }
    public string? DepartmentName { get; set; }
    public long CycleId { get; set; }
    public string? CycleName { get; set; }
    public string? ManagerName { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsSubmitted { get; set; }
    public bool IsLocked { get; set; }
    public string KpiStatus { get; set; } = string.Empty;
    public string SelfStatus { get; set; } = string.Empty;
    public string ManagerStatus { get; set; } = string.Empty;
    public string PeerStatus { get; set; } = string.Empty;
    public string SubordinateStatus { get; set; } = string.Empty;
    public string CommitteeStatus { get; set; } = string.Empty;
}
