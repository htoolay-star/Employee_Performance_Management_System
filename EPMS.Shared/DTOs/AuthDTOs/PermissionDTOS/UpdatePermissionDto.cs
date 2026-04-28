namespace EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS
{
    public record UpdatePermissionDto
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
