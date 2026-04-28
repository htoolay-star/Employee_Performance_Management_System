namespace EPMS.Shared.DTOs.HR
{
    public record PositionDto
    {
        public long Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int LevelId { get; init; }
        public string LevelCode { get; init; } = string.Empty;
        public string LevelName { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}
