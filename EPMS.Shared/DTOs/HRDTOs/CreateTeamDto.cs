namespace EPMS.Shared.DTOs.HR
{
    public record CreateTeamDto
    {
        public string Name { get; init; } = string.Empty;
        public long DepartmentId { get; init; }
    }
}
