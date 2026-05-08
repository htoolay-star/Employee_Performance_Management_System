namespace EPMS.Shared.DTOs.Performance.PositionKPI
{
    public class CreatePositionKPIDto
    {
        public long PositionId { get; set; }
        public long KPIId { get; set; }
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}