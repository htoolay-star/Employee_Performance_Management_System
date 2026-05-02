namespace EPMS.Shared.DTOs.CategoryDTOs;

public record UpdateCategoryDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int? ParentId { get; init; }
}
