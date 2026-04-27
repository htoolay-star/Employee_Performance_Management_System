namespace EPMS.Shared.DTOs.Auth
{
    public record CreatePermissionDto
    {
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
