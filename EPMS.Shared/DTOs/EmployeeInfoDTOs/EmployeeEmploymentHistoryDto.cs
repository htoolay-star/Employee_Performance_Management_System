namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record EmployeeEmploymentHistoryDto
{
    public long Id { get; init; }
    public long EmployeeId { get; init; }
    public long DepartmentId { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public long PositionId { get; init; }
    public string PositionTitle { get; init; } = string.Empty;
    public long? ManagerId { get; init; }
    public string? ManagerName { get; init; }
    public string EmploymentStatus { get; init; } = string.Empty;
    public DateOnly EffectiveDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public string? ChangeReason { get; init; }
    public long? ChangedById { get; init; }
    public string? ChangedByName { get; init; }
}
