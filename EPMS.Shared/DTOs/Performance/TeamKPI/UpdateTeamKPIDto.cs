namespace EPMS.Shared.DTOs.Performance.TeamKPI
{
    public class UpdateTeamKPIDto
    {
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
