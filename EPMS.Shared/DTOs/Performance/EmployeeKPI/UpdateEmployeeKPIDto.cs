namespace EPMS.Shared.DTOs.Performance.EmployeeKPI
{
    public class UpdateEmployeeKPIDto
    {
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
