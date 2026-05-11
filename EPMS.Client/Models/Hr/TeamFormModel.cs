namespace EPMS.Client.Models.Hr
{
    public class TeamFormModel
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? LeadTeamId { get; set; }
        public long? DepartmentId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}