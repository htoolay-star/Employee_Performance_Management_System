namespace EPMS.Shared.DTOs.AppDTOs;

public record CreateNotificationDto
{
    public long ToUserId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? RedirectUrl { get; init; }
}