namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record EmployeeLookupDto
{
    public long Id { get; init; }
    public string StaffNo { get; init; } = string.Empty;
    public string StaffName { get; init; } = string.Empty;
    public long? PositionId { get; init; }
    public bool IsManager { get; init; }
}
