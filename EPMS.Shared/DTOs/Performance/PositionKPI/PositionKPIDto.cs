namespace EPMS.Shared.DTOs.Performance.PositionKPI
{
    public class PositionKPIDto
    {
        public long Id { get; set; }
        public long PositionId { get; set; }
        public long KPIId { get; set; }
        public string KPIName { get; set; } = string.Empty;
        public string KPICode { get; set; } = string.Empty;
        public long PriorityId { get; set; }
        public string PriorityName { get; set; } = string.Empty;
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}