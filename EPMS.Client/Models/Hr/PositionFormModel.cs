namespace EPMS.Client.Models.Hr
{
    public class PositionFormModel
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public long? LevelId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
