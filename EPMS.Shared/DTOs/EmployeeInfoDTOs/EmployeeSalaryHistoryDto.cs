namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record EmployeeSalaryHistoryDto
{
    public long Id { get; init; }
    public long EmployeeId { get; init; }
    public decimal PreviousAmount { get; init; }
    public decimal NewAmount { get; init; }
    public decimal PercentageChange { get; init; }
    public DateOnly EffectiveDate { get; init; }
    public string ChangeReason { get; init; } = string.Empty;
    public long? ApprovedById { get; init; }
    public string? ApprovedByName { get; init; }
    public DateTimeOffset? ApprovedAt { get; init; }
}
