namespace EPMS.Client.Models.Hr
{
    public class PositionFormModel
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public long? LevelId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
