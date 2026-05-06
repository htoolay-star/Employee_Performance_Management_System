namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record CreateEmployeeEmploymentDto
{
    public long EmployeeId { get; init; }
    public long DepartmentId { get; init; }
    public long ParentDepartmentId { get; init; }
    public long PositionId { get; init; }
    public long? TeamId { get; init; }
    public long? DirectManagerId { get; init; }
    public string EmploymentStatus { get; init; } = string.Empty;
    public string? StaffType { get; init; }
    public int? ProbationMonth { get; init; }
    public DateOnly? DateOfAppointment { get; init; }
    public string? Shift { get; init; }
    public string? FingerPrintId { get; init; }
    public bool MobileAttendance { get; init; }
    public string? ProductProject { get; init; }
}
