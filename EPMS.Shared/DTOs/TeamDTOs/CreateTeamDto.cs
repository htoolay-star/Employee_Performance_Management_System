namespace EPMS.Shared.DTOs.TeamDTOs
{
    public record CreateTeamDto
    {
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public long? LeadTeamId { get; init; }
        public long DepartmentId { get; init; }
    }
}
