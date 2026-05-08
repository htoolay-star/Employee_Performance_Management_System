namespace EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;

public record CreateQuestionRatingScaleDto
{
    public string Name { get; init; } = string.Empty;
    public decimal MinScore { get; init; }
    public decimal MaxScore { get; init; }
}