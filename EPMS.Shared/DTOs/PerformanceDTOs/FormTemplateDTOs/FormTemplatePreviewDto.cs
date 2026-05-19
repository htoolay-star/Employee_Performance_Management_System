namespace EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs;

public class FormTemplatePreviewDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FormType { get; set; } = string.Empty;
    public int? QuestionsPerEvaluation { get; set; }
    public long RatingScaleId { get; set; }
    public string? RatingScaleName { get; set; }
    public int? RatingMaxScore { get; set; }
    public bool HasYesNo { get; set; }
    public bool HasComment { get; set; }
    public List<FormTemplatePreviewQuestionDto> Questions { get; set; } = new();
}

public class FormTemplatePreviewQuestionDto
{
    public long Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public long? CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
