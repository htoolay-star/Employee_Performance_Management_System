namespace EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;

public record CreatePositionPIPTemplateDto
{
    public long PositionId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string SuccessCriteria { get; init; } = string.Empty;
    public string? Description { get; init; }
}