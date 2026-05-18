namespace EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs
{
    public class FormTemplateDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FormType { get; set; } = string.Empty;
        public int? QuestionsPerEvaluation { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int QuestionCount { get; set; }
    }
}