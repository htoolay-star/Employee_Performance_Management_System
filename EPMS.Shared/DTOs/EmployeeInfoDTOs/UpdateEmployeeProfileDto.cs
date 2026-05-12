namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record UpdateEmployeeProfileDto
{
    public string StaffName { get; init; } = string.Empty;
    public string? OtherName { get; init; }
    public string? NRCNo { get; init; }
    public string? Gender { get; init; }
    public string? Race { get; init; }
    public string? Religion { get; init; }
    public string? Nationality { get; init; }
    public string? BirthPlace { get; init; }
    public string? PassportNo { get; init; }
    public string? LabourRegistrationNo { get; init; }
    public string? EmailAddress { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public DateOnly? PassportExpireDate { get; init; }
    public string? WorkPermitNo { get; init; }
    public DateOnly? WorkPermitValidDate { get; init; }
    public DateOnly? WorkPermitExpireDate { get; init; }
    public string? ProfilePictureUrl { get; init; }
    public string? ProfileThumbnailUrl { get; init; }
    public string? AdditionalData { get; init; }
}
