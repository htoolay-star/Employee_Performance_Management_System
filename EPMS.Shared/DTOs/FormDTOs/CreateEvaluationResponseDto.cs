namespace EPMS.Shared.DTOs.FormDTOs;

public record CreateEvaluationResponseDto
{
    public long AppraisalId { get; init; }
    public long TemplateId { get; init; }
    public long QuestionId { get; init; }
    public long EvaluatorId { get; init; }
    public string EvaluatorRole { get; init; }
    public bool IsAnonymous { get; init; }
    public bool? YesNoAnswer { get; init; }
    public int? RatingValue { get; init; }
    public string? Comment { get; init; }
}