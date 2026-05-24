namespace EPMS.Client.Models.Performance
{
    public class QuestionRatingScaleFormModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<QuestionRatingScaleLevelFormModel> Levels { get; set; } = new();
    }
}
