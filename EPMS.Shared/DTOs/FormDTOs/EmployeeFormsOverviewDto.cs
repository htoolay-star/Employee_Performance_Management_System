namespace EPMS.Shared.DTOs.FormDTOs;

public class EmployeeFormsOverviewDto
{
    public long AppraisalId { get; set; }
    public long? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? PositionName { get; set; }
    public string? DepartmentName { get; set; }
    public string? TeamName { get; set; }
    public string? ManagerName { get; set; }
    public string? CycleName { get; set; }
    public List<FormEntryDto> Forms { get; set; } = new();
}

public class FormEntryDto
{
    public string FormType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsSubmitted { get; set; }
    public bool IsLocked { get; set; }
    public bool CanFill { get; set; }
    public long? EvaluatorId { get; set; }
    public string? EvaluatorName { get; set; }
    public decimal? Score { get; set; }
}
