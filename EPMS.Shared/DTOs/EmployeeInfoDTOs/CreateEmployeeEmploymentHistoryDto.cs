namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record CreateEmployeeEmploymentHistoryDto
{
    public long EmployeeId { get; init; }
    public long DepartmentId { get; init; }
    public long PositionId { get; init; }
    public long? ManagerId { get; init; }
    public string EmploymentStatus { get; init; } = string.Empty;
    public DateOnly EffectiveDate { get; init; }
    public string? ChangeReason { get; init; }
    public long? ChangedById { get; init; }
}
