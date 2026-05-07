namespace EPMS.Shared.DTOs.TagDTOs;

public record UpdateTagDto
{
    public string Name { get; init; } = string.Empty;
    public string? Module { get; init; }
}