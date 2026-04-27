namespace EPMS.Shared.DTOs.Auth
{
    public record UpdatePermissionDto
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
