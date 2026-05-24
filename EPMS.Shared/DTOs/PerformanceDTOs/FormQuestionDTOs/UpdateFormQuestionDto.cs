namespace EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;

public record UpdateFormQuestionDto
{
    public string? QuestionText { get; init; }
    public int? Sequence { get; init; }
    public long? CategoryId { get; init; }
}