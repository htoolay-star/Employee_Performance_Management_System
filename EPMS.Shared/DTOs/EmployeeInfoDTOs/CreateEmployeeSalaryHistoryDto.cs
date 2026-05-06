namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record CreateEmployeeSalaryHistoryDto
{
    public long EmployeeId { get; init; }
    public decimal PreviousAmount { get; init; }
    public decimal NewAmount { get; init; }
    public DateOnly EffectiveDate { get; init; }
    public string ChangeReason { get; init; } = string.Empty;
    public long? ApprovedById { get; init; }
}
