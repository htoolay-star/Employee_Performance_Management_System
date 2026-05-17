namespace EPMS.Shared.DTOs.FormDTOs;

public class AppraisalDetailFillDto
{
    public long? KPIId { get; set; }
    public string KPIName { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public decimal Weightage { get; set; }
    public string? TargetValue { get; set; }
    public string ScoringDirection { get; set; } = string.Empty;
    public string? ActualValue { get; set; }
    public decimal Score { get; set; }
    public decimal WeightedScore { get; set; }
    public string? Remarks { get; set; }
}
