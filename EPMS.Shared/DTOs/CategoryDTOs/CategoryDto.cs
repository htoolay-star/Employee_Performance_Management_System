namespace EPMS.Shared.DTOs.CategoryDTOs;

public record CategoryDto
{
    public long Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public long? ParentId { get; init; }
    public string? ParentName { get; set; }
    public bool IsActive { get; init; } = true;
}
