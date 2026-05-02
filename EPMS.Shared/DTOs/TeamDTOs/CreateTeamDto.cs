namespace EPMS.Shared.DTOs.TeamDTOs
{
    public record CreateTeamDto
    {
        public string Name { get; init; } = string.Empty;
        public long DepartmentId { get; init; }
    }
}
