namespace EPMS.Shared.DTOs.PerformanceDTOs.KPIWeightPriorityDTOs;

public record UpdateKPIWeightPriorityDto
{
    public decimal? MinWeight { get; init; }
    public decimal? MaxWeight { get; init; }
    public string? ColorCode { get; init; }
    public bool? IsActive { get; init; }
}
