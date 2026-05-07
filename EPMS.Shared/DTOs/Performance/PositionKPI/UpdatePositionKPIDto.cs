namespace EPMS.Shared.DTOs.Performance.PositionKPI
{
    public class UpdatePositionKPIDto
    {
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}