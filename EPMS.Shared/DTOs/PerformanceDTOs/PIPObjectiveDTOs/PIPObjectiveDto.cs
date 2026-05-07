namespace EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;

public class PIPObjectiveDto
{
    public long Id { get; set; }
    public long PIPId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SuccessCriteria { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ManagerComment { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}