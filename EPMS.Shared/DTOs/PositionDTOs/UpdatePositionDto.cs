namespace EPMS.Shared.DTOs.PositionDTOs
{
    public record UpdatePositionDto
    {
        public string Title { get; init; } = string.Empty;
        public int LevelId { get; init; }
        public bool IsActive { get; init; } = true;
    }
}
