namespace EPMS.Shared.DTOs.PermissionDTOS
{
    public record UpdatePermissionDto
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
