namespace EPMS.Shared.DTOs.PerformanceDTOs.PositionPIPTemplateDTOs;

public class PositionPIPTemplateDto
{
    public long Id { get; set; }
    public long PositionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SuccessCriteria { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}