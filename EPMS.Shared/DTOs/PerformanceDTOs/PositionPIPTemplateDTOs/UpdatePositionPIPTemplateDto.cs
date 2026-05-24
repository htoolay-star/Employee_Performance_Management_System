namespace EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;

public record UpdatePositionPIPTemplateDto
{
    public string? Title { get; init; }
    public string? SuccessCriteria { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
}