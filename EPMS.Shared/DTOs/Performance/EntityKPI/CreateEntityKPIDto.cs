namespace EPMS.Shared.DTOs.Performance.EntityKPI
{
    public class CreateEntityKPIDto
    {
        public string EntityType { get; set; } = string.Empty;
        public long EntityId { get; set; }
        public long KPIId { get; set; }
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
