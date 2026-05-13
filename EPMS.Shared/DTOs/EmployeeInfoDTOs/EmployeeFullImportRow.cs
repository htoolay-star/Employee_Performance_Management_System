namespace EPMS.Shared.DTOs.EmployeeInfoDTOs;

public record EmployeeFullImportRow
{
    public string StaffNo { get; init; } = string.Empty;
    public string StaffName { get; init; } = string.Empty;
    public string? OtherName { get; init; }
    public string? Gender { get; init; }
    public string? NRCNo { get; init; }
    public string? Race { get; init; }
    public string? Religion { get; init; }
    public string? Nationality { get; init; }
    public string? BirthPlace { get; init; }
    public string? EmailAddress { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public string? PassportNo { get; init; }
    public DateOnly? PassportExpireDate { get; init; }
    public string? LabourRegistrationNo { get; init; }
    public string? WorkPermitNo { get; init; }
    public DateOnly? WorkPermitValidDate { get; init; }
    public DateOnly? WorkPermitExpireDate { get; init; }

    public string? EmploymentStatus { get; init; }
    public string? StaffType { get; init; }
    public int? ProbationMonth { get; init; }
    public string? Shift { get; init; }
    public DateOnly? DateOfAppointment { get; init; }
    public DateOnly? DateOfConfirmation { get; init; }
    public DateOnly? DateOfPromotion { get; init; }
    public DateOnly? DateOfTermination { get; init; }
    public DateOnly? DateOfTransfer { get; init; }
    public DateOnly? DateOfDemotion { get; init; }
    public DateOnly? DateOfTitleChange { get; init; }
    public DateOnly? DateOfIncrement { get; init; }
    public string? DepartmentName { get; init; }
    public string? ParentDepartmentName { get; init; }
    public string? TeamName { get; init; }
    public string? PositionName { get; init; }
    public string? DirectManagerStaffNo { get; init; }
    public string? ProductProject { get; init; }
    public string? FingerPrintId { get; init; }
    public bool MobileAttendance { get; init; }

    public string? ContactAddress { get; init; }
    public string? PermanentAddress { get; init; }
    public string? PhoneNo { get; init; }
    public string? PermanentPhoneNo { get; init; }
    public string? PresentPhoneNo { get; init; }
    public string? InternalPhoneNo { get; init; }
    public string? EmergencyMobileNo { get; init; }
    public string? RelationWithEmergencyContact { get; init; }

    public string? MaritalStatus { get; init; }
    public string? SpouseName { get; init; }
    public string? SpouseNRCNo { get; init; }
    public string? SpouseOccupation { get; init; }
    public string? FatherName { get; init; }
    public string? FatherNRCNo { get; init; }
    public string? FatherOccupation { get; init; }

    public decimal? Salary { get; init; }
    public string? Currency { get; init; }
    public string? PayType { get; init; }
    public DateOnly? DateOfPayTypeChanged { get; init; }
    public DateOnly? DateOfSalaryChanged { get; init; }
    public DateOnly? DateOfCurrencyChange { get; init; }
    public string? CostAllocate { get; init; }
    public string? PayByBacklog { get; init; }
    public string? TaxStatus { get; init; }
    public string? TaxNo { get; init; }
    public string? SSBStatus { get; init; }
    public string? SSCBNo { get; init; }
    public int? ComplianceEarnedPoints { get; init; }
    public int? ComplianceBalancePoints { get; init; }
}