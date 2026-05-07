namespace EPMS.Shared.DTOs.PerformanceDTOs.PIPObjectiveDTOs;

public record UpdatePIPObjectiveDto
{
    public string? Title { get; init; }
    public string? SuccessCriteria { get; init; }
    public string? Description { get; init; }
    public string? Status { get; init; }
    public string? ManagerComment { get; init; }
}