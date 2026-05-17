namespace EPMS.Client.Models.Performance
{
    public class FormQuestionFormModel
    {
        public long Id { get; set; }

        public long TemplateId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public int Sequence { get; set; }

        public long? CategoryId { get; set; }

        public long? RatingScaleId { get; set; }

        public bool HasYesNo { get; set; }

        public bool HasComment { get; set; }
    }
}
