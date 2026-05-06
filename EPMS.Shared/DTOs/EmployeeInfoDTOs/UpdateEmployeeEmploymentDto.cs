namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record UpdateEmployeeEmploymentDto
{
    public long DepartmentId { get; init; }
    public long ParentDepartmentId { get; init; }
    public long PositionId { get; init; }
    public long? TeamId { get; init; }
    public long? DirectManagerId { get; init; }
    public string EmploymentStatus { get; init; } = string.Empty;
    public string? StaffType { get; init; }
    public int? ProbationMonth { get; init; }
    public DateOnly? DateOfAppointment { get; init; }
    public DateOnly? DateOfConfirmation { get; init; }
    public DateOnly? DateOfPromotion { get; init; }
    public DateOnly? DateOfTermination { get; init; }
    public DateOnly? DateOfTransfer { get; init; }
    public DateOnly? DateOfDemotion { get; init; }
    public DateOnly? DateOfTitleChange { get; init; }
    public string? Shift { get; init; }
    public string? FingerPrintId { get; init; }
    public bool MobileAttendance { get; init; }
    public DateOnly? DateOfIncrement { get; init; }
    public string? ProductProject { get; init; }
}
