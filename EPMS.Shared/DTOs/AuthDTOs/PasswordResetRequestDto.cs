namespace EPMS.Shared.DTOs.Auth;

public record PasswordResetRequestDto
{
    public long Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? StaffName { get; init; }
    public DateTimeOffset RequestedAt { get; init; }
    public string Status { get; init; } = string.Empty;
}
