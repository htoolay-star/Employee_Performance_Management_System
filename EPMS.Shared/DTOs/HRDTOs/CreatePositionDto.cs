namespace EPMS.Shared.DTOs.HR
{
    public record CreatePositionDto
    {
        public string Title { get; init; } = string.Empty;
        public int LevelId { get; init; }
    }
}
