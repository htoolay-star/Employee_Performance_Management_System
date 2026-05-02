namespace EPMS.Shared.DTOs.TeamDTOs
{
    public record TeamDto
    {
        public long Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public long DepartmentId { get; init; }
        public bool IsActive { get; init; }
    }
}
