namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record CreateFullEmployeeDto
{
    public CreateEmployeeProfileDto Profile { get; init; } = null!;
    public CreateEmployeeEmploymentDto? Employment { get; init; }
    public CreateEmployeeContactDto? Contact { get; init; }
    public CreateEmployeeFamilyInfoDto? Family { get; init; }
    public CreateEmployeePayrollInfoDto? Payroll { get; init; }
}
