namespace EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;

public record KPIWeightPriorityDto
{
    public long Id { get; init; }
    public string LevelName { get; init; } = string.Empty;
    public decimal MinWeight { get; init; }
    public decimal MaxWeight { get; init; }
    public string? ColorCode { get; init; }
    public bool IsActive { get; init; }
}
