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
    public List<AppraisalDetailFillDto> Details { get; set; } = new();
}
