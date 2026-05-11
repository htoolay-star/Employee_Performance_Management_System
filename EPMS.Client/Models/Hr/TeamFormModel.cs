namespace EPMS.Client.Models.Hr
{
    public class TeamFormModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long? DepartmentId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}