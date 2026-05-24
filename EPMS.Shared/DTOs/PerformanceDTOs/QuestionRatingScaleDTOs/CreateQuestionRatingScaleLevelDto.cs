namespace EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;

public class CreateQuestionRatingScaleLevelDto
{
    public int Rating { get; init; }
    public decimal MinScore { get; init; }
    public decimal MaxScore { get; init; }
}
