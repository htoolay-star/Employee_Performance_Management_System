namespace EPMS.Client.Models.Hr
{
    public class CategoryFormModel
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? ParentId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
