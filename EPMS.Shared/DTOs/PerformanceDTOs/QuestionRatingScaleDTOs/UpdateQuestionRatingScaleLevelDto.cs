namespace EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;

public class UpdateQuestionRatingScaleLevelDto
{
    public long? Id { get; init; }
    public int Rating { get; init; }
    public decimal MinScore { get; init; }
    public decimal MaxScore { get; init; }
}
