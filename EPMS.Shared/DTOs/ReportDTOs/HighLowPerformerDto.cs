namespace EPMS.Shared.DTOs.ReportDTOs;

public class HighLowPerformerDto
{
    public int Rank { get; set; }
    public long EmployeeId { get; set; }
    public string StaffNo { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string? DepartmentName { get; set; }
    public string? PositionName { get; set; }
    public decimal? TotalScore { get; set; }
    public string? RatingLabel { get; set; }
    public string? CycleName { get; set; }
    public string? PIPStatus { get; set; }
}
