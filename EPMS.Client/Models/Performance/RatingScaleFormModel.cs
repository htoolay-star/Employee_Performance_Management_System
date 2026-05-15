namespace EPMS.Client.Models.Performance
{
    public class RatingScaleFormModel
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal MinScore { get; set; }
        public decimal MaxScore { get; set; }
        public string? PerformanceLevel { get; set; }
        public string? PromotionEligibility { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
