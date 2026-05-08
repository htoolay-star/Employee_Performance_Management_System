namespace EPMS.Shared.DTOs.FormDTOs;

public record UpdateAppraisalRecommendationDto
{
    public string? Type { get; init; }
    public string? Reason { get; init; }
    public string? ProposedValue { get; init; }
    public string? Priority { get; init; }
}