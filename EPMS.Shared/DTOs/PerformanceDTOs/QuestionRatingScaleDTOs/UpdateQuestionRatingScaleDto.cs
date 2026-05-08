namespace EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;

public record UpdateQuestionRatingScaleDto
{
    public string? Name { get; init; }
    public decimal? MinScore { get; init; }
    public decimal? MaxScore { get; init; }
}