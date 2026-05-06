namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record CreateEmployeePayrollInfoDto
{
    public long EmployeeId { get; init; }
    public decimal Salary { get; init; }
    public string? Currency { get; init; }
    public string? PayType { get; init; }
    public string? CostAllocate { get; init; }
    public string? PayByBacklog { get; init; }
    public string? TaxStatus { get; init; }
    public string? TaxNo { get; init; }
    public string? SSBStatus { get; init; }
    public string? SSCBNo { get; init; }
    public int? ComplianceEarnedPoints { get; init; }
    public int? ComplianceBalancePoints { get; init; }
}
