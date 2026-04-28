namespace EPMS.Shared.PositionDTOs
{
    public class PositionResponseDto
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;

        public int LevelId { get; set; }
        public string LevelName { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

        public int? TeamId { get; set; }
        public string? TeamName { get; set; }

        public bool IsActive { get; set; }
    }
}