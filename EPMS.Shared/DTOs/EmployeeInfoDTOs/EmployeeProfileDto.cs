namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record EmployeeProfileDto
{
    public long Id { get; init; }
    public Guid PublicId { get; init; }
    public long? UserId { get; init; }
    public string StaffNo { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string? LastName { get; init; }
    public string? OtherName { get; init; }
    public string? NRCNo { get; init; }
    public string? Gender { get; init; }
    public string? Race { get; init; }
    public string? Religion { get; init; }
    public string? Nationality { get; init; }
    public string? BirthPlace { get; init; }
    public string? PassportNo { get; init; }
    public string? LabourRegistrationNo { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public DateOnly? PassportExpireDate { get; init; }
    public string? WorkPermitNo { get; init; }
    public DateOnly? WorkPermitValidDate { get; init; }
    public DateOnly? WorkPermitExpireDate { get; init; }
    public string? ProfilePictureUrl { get; init; }
    public string? ProfileThumbnailUrl { get; init; }
    public string? AdditionalData { get; init; }
}
