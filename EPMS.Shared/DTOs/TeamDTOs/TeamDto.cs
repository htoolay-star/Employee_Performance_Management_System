namespace EPMS.Shared.DTOs.TeamDTOs
{
    public record TeamDto
    {
        public long Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public long? LeadTeamId { get; init; }
        public string? LeadTeamName { get; init; }
        public long DepartmentId { get; init; }
        public string DepartmentCode { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}
