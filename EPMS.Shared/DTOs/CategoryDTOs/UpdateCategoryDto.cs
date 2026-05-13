namespace EPMS.Shared.DTOs.CategoryDTOs;

public record UpdateCategoryDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public long? ParentId { get; init; }
    public bool IsActive { get; init; } = true;
}
