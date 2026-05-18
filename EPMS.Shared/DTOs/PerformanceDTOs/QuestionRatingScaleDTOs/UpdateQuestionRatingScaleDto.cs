namespace EPMS.Shared.DTOs.PerformanceDTOs.QuestionRatingScaleDTOs;

public record UpdateQuestionRatingScaleDto
{
    public string? Name { get; init; }
    public bool? IsActive { get; init; }
    public List<UpdateQuestionRatingScaleLevelDto> Levels { get; init; } = new();
}
