namespace EPMS.Shared.DTOs.FormDTOs;

public record SubmitRoleRequestDto
{
    public long AppraisalId { get; init; }
    public string Role { get; init; } = string.Empty;
}
