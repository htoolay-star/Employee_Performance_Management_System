namespace EPMS.Shared.DTOs.FormDTOs;

public record UpdateEvaluationResponseDto
{
    public bool? YesNoAnswer { get; init; }
    public int? RatingValue { get; init; }
    public string? Comment { get; init; }
}