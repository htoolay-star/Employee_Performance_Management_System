namespace EPMS.Shared.DTOs.ReportDTOs;

public class EmployeeSummaryReportDto
{
    public long EmployeeId { get; set; }
    public string StaffNo { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string? DepartmentName { get; set; }
    public string? PositionName { get; set; }
    public string? CycleName { get; set; }
    public decimal? TotalScore { get; set; }
    public decimal? KpiScore { get; set; }
    public decimal? SelfScore { get; set; }
    public decimal? ThreeSixtyScore { get; set; }
    public decimal? AppraisalScore { get; set; }
    public string? RatingLabel { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset? FinalizedDate { get; set; }
}
