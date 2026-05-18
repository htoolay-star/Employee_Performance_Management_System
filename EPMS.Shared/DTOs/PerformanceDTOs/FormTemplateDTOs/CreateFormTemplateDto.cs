namespace EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs
{
    public class CreateFormTemplateDto
    {
        public string Name { get; set; } = string.Empty;
        public string FormType { get; set; } = string.Empty;
        public int? QuestionsPerEvaluation { get; set; }
        public bool HasYesNo { get; init; }
        public bool HasComment { get; init; }
    }
}