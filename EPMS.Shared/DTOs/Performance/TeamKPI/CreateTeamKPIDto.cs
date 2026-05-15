namespace EPMS.Shared.DTOs.Performance.TeamKPI
{
    public class CreateTeamKPIDto
    {
        public long TeamId { get; set; }
        public long KPIId { get; set; }
        public long PriorityId { get; set; }
        public decimal Weightage { get; set; }
        public string? TargetValue { get; set; }
        public string? TargetUnit { get; set; }
    }
}
