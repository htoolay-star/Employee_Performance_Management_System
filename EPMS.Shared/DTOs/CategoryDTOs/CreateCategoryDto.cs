namespace EPMS.Shared.DTOs.CategoryDTOs;

public record CreateCategoryDto
{
    public string Module { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int? ParentId { get; init; }
}
