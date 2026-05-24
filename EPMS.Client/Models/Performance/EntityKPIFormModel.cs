namespace EPMS.Client.Models.Performance
{
    public class EntityKPIFormModel
    {
        public long Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public long? EntityId { get; set; }
        public long? KPIId { get; set; }
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public decimal? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
