namespace EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs
{
    public class UpdateFormTemplateDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FormType { get; set; } = string.Empty;
        public int? QuestionsPerEvaluation { get; set; }
    }
}