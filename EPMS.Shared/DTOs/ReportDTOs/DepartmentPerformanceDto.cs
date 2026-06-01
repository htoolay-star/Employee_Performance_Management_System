namespace EPMS.Shared.DTOs.ReportDTOs;

public class DepartmentPerformanceDto
{
    public long DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? DeptHeadName { get; set; }
    public int EmployeeCount { get; set; }
    public int EvaluatedCount { get; set; }
    public double? AvgTotalScore { get; set; }
    public decimal? MinScore { get; set; }
    public decimal? MaxScore { get; set; }
    public int HighPerformerCount { get; set; }
    public int LowPerformerCount { get; set; }
}
