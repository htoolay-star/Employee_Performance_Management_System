namespace EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs
{
    public class FormTemplateDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FormType { get; set; } = string.Empty;
        public long RatingScaleId { get; set; }
        public string RatingScaleName { get; set; } = string.Empty;
        public int? QuestionsPerEvaluation { get; set; }
        public bool IsActive { get; set; }
        public bool HasYesNo { get; set; }
        public bool HasComment { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int QuestionCount { get; set; }
    }
}