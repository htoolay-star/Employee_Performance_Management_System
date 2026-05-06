namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record EmployeeProfileDetailDto : EmployeeProfileDto
{
    public EmployeeEmploymentDto? Employment { get; init; }
    public EmployeeContactDto? Contact { get; init; }
    public EmployeePayrollInfoDto? PayrollInfo { get; init; }
    public EmployeeFamilyInfoDto? FamilyInfo { get; init; }
}
