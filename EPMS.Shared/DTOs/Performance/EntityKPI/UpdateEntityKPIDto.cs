namespace EPMS.Shared.DTOs.Performance.EntityKPI
{
    public class UpdateEntityKPIDto
    {
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public decimal? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
