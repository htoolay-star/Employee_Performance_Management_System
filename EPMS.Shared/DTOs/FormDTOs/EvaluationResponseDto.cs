namespace EPMS.Shared.DTOs.FormDTOs;

public record EvaluationResponseDto(
    long Id,
    long AppraisalId,
    string? AppraisalEmployeeName,
    long TemplateId,
    string? TemplateName,
    long QuestionId,
    string? QuestionText,
    long EvaluatorId,
    string? EvaluatorName,
    string EvaluatorRole,
    bool IsAnonymous,
    bool? YesNoAnswer,
    int? RatingValue,
    string? QuestionComment,
    DateTimeOffset CreatedAt);