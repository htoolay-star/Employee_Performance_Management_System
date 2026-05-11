namespace EPMS.Client.Models.Hr
{
    public class DepartmentFormModel
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? DeptHeadId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}