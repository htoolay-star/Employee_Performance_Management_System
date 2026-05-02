namespace EPMS.Shared.DTOs.LevelDTOs;

public record UpdateLevelDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;
}
