namespace EPMS.Shared.DTOs.Performance.EntityKPI
{
    public class EntityKPIDto
    {
        public long Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public long EntityId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public long KPIId { get; set; }
        public string KPIName { get; set; } = string.Empty;
        public string KPICode { get; set; } = string.Empty;
        public long PriorityId { get; set; }
        public string PriorityName { get; set; } = string.Empty;
        public decimal Weightage { get; set; }
        public decimal? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
