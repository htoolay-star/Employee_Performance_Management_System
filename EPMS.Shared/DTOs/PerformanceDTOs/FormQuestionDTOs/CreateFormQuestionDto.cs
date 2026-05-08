namespace EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;

public record CreateFormQuestionDto
{
    public long TemplateId { get; init; }
    public string QuestionText { get; init; }
    public int Sequence { get; init; }
    public bool HasYesNo { get; init; }
    public bool HasComment { get; init; }
    public long? CategoryId { get; init; }
    public long? RatingScaleId { get; init; }
}