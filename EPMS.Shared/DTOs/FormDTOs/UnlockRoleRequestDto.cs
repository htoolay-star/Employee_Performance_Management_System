namespace EPMS.Shared.DTOs.FormDTOs;

public record UnlockRoleRequestDto
{
    public string Role { get; init; } = string.Empty;
}
