namespace EPMS.Shared.DTOs.Performance.DeptKPI
{
    public class CreateDeptKPIDto
    {
        public long DeptId { get; set; }
        public long KPIId { get; set; }
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
