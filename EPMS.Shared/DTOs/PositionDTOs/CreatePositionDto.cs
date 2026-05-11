namespace EPMS.Shared.DTOs.PositionDTOs
{
    public record CreatePositionDto
    {
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public long LevelId { get; init; }
        public string? Description { get; init; }
    }
}
