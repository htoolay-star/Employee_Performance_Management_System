namespace EPMS.Shared.DTOs.Performance.EmployeeKPI
{
    public class CreateEmployeeKPIDto
    {
        public long EmployeeId { get; set; }
        public long CycleId { get; set; }
        public long KPIId { get; set; }
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
