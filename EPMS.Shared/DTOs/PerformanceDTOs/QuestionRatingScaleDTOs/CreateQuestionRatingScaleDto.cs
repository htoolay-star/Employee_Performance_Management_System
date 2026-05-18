namespace EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;

public record CreateQuestionRatingScaleDto
{
    public string Name { get; init; } = string.Empty;
    public List<CreateQuestionRatingScaleLevelDto> Levels { get; init; } = new();
}
