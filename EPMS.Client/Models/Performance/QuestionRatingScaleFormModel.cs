namespace EPMS.Client.Models.Performance
{
    public class QuestionRatingScaleFormModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MinScore { get; set; }
        public decimal MaxScore { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
