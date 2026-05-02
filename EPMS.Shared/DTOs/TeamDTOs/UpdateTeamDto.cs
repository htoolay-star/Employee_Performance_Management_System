namespace EPMS.Shared.DTOs.TeamDTOs
{
    public record UpdateTeamDto
    {
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;
    }
}
