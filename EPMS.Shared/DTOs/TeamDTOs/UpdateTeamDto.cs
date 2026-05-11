namespace EPMS.Shared.DTOs.TeamDTOs
{
    public record UpdateTeamDto
    {
        public string Name { get; init; } = string.Empty;
        public long? DepartmentId { get; init; }
        public bool IsActive { get; init; } = true;
    }
}
