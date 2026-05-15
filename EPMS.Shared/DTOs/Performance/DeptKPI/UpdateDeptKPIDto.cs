namespace EPMS.Shared.DTOs.Performance.DeptKPI
{
    public class UpdateDeptKPIDto
    {
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
