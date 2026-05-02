namespace EPMS.Shared.DTOs.TagDTOs;

public record CreateTagDto
{
    public string Name { get; init; } = string.Empty;
    public string? Module { get; init; }
}
