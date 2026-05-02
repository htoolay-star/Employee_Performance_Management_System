namespace EPMS.Shared.DTOs.TagDTOs;

public record TagDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Module { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}
