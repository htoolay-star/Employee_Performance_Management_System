namespace EPMS.Shared.DTOs.ReportDTOs;

public class PromotionRecommendationDto
{
    public long EmployeeId { get; set; }
    public string StaffNo { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string? DepartmentName { get; set; }
    public string? PositionName { get; set; }
    public string? LevelName { get; set; }
    public decimal? TotalScore { get; set; }
    public string? RatingLabel { get; set; }
    public string? PromotionEligibility { get; set; }
    public string? CycleName { get; set; }
    public int? TenureMonths { get; set; }
}
