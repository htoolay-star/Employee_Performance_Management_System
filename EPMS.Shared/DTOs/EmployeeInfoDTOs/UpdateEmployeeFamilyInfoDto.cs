namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record UpdateEmployeeFamilyInfoDto
{
    public string? MaritalStatus { get; init; }
    public string? SpouseName { get; init; }
    public string? SpouseNRCNo { get; init; }
    public string? SpouseOccupation { get; init; }
    public string? FatherName { get; init; }
    public string? FatherNRCNo { get; init; }
    public string? FatherOccupation { get; init; }
}
