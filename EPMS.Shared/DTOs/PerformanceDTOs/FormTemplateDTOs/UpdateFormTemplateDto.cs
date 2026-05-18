namespace EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs
{
    public class UpdateFormTemplateDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? FormType { get; set; }
        public int? QuestionsPerEvaluation { get; set; }
        public bool? HasYesNo { get; init; }
        public bool? HasComment { get; init; }
        public bool? IsActive { get; init; }
    }
}