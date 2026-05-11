namespace EPMS.Shared.DTOs.TeamDTOs
{
    public record TeamLookupDto
    {
        public long Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}