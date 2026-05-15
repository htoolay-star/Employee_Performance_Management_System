namespace EPMS.Client.Models.Performance
{
    public class KPIWeightPriorityFormModel
    {
        public long Id { get; set; }
        public string LevelName { get; set; } = string.Empty;
        public decimal MinWeight { get; set; }
        public decimal MaxWeight { get; set; }
        public string? ColorCode { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
