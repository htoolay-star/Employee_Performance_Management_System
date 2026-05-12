namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record EmployeeProfileGridItemDto
{
    public long Id { get; init; }
    public string StaffNo { get; init; } = string.Empty;
    public string StaffName { get; init; } = string.Empty;
    public string? PositionName { get; init; }
    public string? DepartmentName { get; init; }
    public string? TeamName { get; init; }
    public string? EmploymentStatus { get; init; }
    public int RowIndex { get; init; }
}
