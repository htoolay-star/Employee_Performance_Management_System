namespace EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;

public record UpdateRatingScaleDto
{
    public decimal? MinScore { get; init; }
    public decimal? MaxScore { get; init; }
    public string? PromotionEligibility { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
}
