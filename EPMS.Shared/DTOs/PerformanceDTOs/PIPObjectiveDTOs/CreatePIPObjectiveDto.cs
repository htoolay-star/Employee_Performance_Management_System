namespace EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;

public record CreatePIPObjectiveDto
{
    public long PIPId { get; init; }
    public string Title { get; init; }
    public string SuccessCriteria { get; init; }
    public string? Description { get; init; }
}