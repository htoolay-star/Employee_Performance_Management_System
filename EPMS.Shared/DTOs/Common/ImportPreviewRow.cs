namespace EPMS.Shared.DTOs.Common;

using EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record ImportPreviewRow
{
    public int RowNumber { get; init; }
    public string StaffNo { get; init; } = string.Empty;
    public string StaffName { get; init; } = string.Empty;
    public string? EmailAddress { get; init; }
    public string? DepartmentName { get; init; }
    public string? TeamName { get; init; }
    public string? PositionName { get; init; }
    public string? EmploymentStatus { get; init; }
    public bool IsValid { get; init; }
    public List<string> Errors { get; init; } = [];
    public EmployeeFullImportRow? Data { get; init; }
}
