namespace EPMS.Client.Models.Performance
{
    public class QuestionRatingScaleLevelFormModel
    {
        public long? Id { get; set; }
        public int Rating { get; set; }
        public decimal MinScore { get; set; }
        public decimal MaxScore { get; set; }
    }
}
