namespace EPMS.Shared.DTOs.PerformanceDTOs.FormQuestionDTOs;

public record FormQuestionDto(
    long Id,
    long TemplateId,
    string QuestionText,
    int Sequence,
    bool HasYesNo,
    bool HasComment,
    long? CategoryId,
    string? CategoryName,
    long? RatingScaleId,
    string? RatingScaleName);