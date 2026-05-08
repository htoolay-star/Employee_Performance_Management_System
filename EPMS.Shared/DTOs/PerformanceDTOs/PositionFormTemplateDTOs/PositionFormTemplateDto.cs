namespace EPMS.Shared.DTOs.PerformanceDTOs.PositionFormTemplateDTOs;

public class PositionFormTemplateDto
{
    public long Id { get; set; }
    public long PositionId { get; set; }
    public long FormTemplateId { get; set; }
    public bool IsMandatory { get; set; }
}