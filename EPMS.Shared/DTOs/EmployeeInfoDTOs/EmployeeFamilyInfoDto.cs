namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record EmployeeFamilyInfoDto
{
    public long Id { get; init; }
    public long EmployeeId { get; init; }
    public string? MaritalStatus { get; init; }
    public string? SpouseName { get; init; }
    public string? SpouseNRCNo { get; init; }
    public string? SpouseOccupation { get; init; }
    public string? FatherName { get; init; }
    public string? FatherNRCNo { get; init; }
    public string? FatherOccupation { get; init; }
}
