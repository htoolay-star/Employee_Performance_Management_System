namespace EPMS.Shared.DTOs.PositionDTOs
{
    public record PositionDto
    {
        public long Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public long LevelId { get; init; }
        public string LevelCode { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}
