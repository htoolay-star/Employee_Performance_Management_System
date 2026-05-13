namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record UpdateEmployeeContactDto
{
    public string? ContactAddress { get; init; }
    public string? PermanentAddress { get; init; }
    public string? PhoneNo { get; init; }
    public string? PermanentPhoneNo { get; init; }
    public string? PresentPhoneNo { get; init; }
    public string? InternalPhoneNo { get; init; }
    public string? EmergencyMobileNo { get; init; }
    public string? RelationWithEmergencyContact { get; init; }
}
