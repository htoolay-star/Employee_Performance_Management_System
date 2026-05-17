namespace EPMS.Client.Models.Performance
{
    public class KPIMasterFormModel
    {
        public long Id { get; set; }
        public long? CategoryId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ScoringDirection { get; set; } = "HigherIsBetter";
        public bool IsActive { get; set; } = true;
    }
}
