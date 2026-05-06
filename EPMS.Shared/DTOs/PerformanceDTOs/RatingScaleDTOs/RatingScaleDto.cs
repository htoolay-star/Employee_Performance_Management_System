namespace EPMS.Shared.DTOs.PerformanceDTOs.RatingScaleDTOs;

public record RatingScaleDto
{
    public long Id { get; init; }
    public int Rating { get; init; }
    public string Label { get; init; } = string.Empty;
    public decimal MinScore { get; init; }
    public decimal MaxScore { get; init; }
    public string? PerformanceLevel { get; init; }
    public string? PromotionEligibility { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
