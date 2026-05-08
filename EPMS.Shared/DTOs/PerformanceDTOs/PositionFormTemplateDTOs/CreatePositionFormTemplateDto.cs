namespace EPMS.Shared.DTOs.PerformanceDTOs.PositionFormTemplateDTOs;

public record CreatePositionFormTemplateDto
{
    public long PositionId { get; init; }
    public long FormTemplateId { get; init; }
    public bool IsMandatory { get; init; } = true;
}