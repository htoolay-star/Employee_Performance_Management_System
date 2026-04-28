namespace EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS
{
    public record PermissionDto
    {
        public int Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool IsActive { get; init; }
    }
}
