namespace EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;

public record FormQuestionDto(
    long Id,
    long TemplateId,
    string QuestionText,
    int Sequence,
    long? CategoryId,
    string? CategoryName,
    long RatingScaleId,
    string? RatingScaleName);