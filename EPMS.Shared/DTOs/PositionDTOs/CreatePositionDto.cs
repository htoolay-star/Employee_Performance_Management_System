namespace EPMS.Shared.DTOs.PositionDTOs
{
    public record CreatePositionDto
    {
        public string Title { get; init; } = string.Empty;
        public long LevelId { get; init; }
    }
}
