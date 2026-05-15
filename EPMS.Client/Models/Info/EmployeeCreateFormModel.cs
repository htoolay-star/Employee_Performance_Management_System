namespace EPMS.Client.Models.Info
{
    public class EmployeeCreateFormModel
    {
        // ── Profile ──
        public string StaffNo { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        public string? NRCNo { get; set; }
        public string? Gender { get; set; }
        public string? Race { get; set; }
        public string? Religion { get; set; }
        public string? Nationality { get; set; }
        public string? BirthPlace { get; set; }
        public string? PassportNo { get; set; }
        public string? LabourRegistrationNo { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string? WorkPermitNo { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? PassportExpireDate { get; set; }
        public DateOnly? WorkPermitValidDate { get; set; }
        public DateOnly? WorkPermitExpireDate { get; set; }

        public DateTime? DateOfBirthProxy
        {
            get => DateOfBirth?.ToDateTime(TimeOnly.MinValue);
            set => DateOfBirth = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }
        public DateTime? PassportExpireDateProxy
        {
            get => PassportExpireDate?.ToDateTime(TimeOnly.MinValue);
            set => PassportExpireDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }
        public DateTime? WorkPermitValidDateProxy
        {
            get => WorkPermitValidDate?.ToDateTime(TimeOnly.MinValue);
            set => WorkPermitValidDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }
        public DateTime? WorkPermitExpireDateProxy
        {
            get => WorkPermitExpireDate?.ToDateTime(TimeOnly.MinValue);
            set => WorkPermitExpireDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

        // ── Employment ──
        public long DepartmentId { get; set; }
        public long ParentDepartmentId { get; set; }
        public long PositionId { get; set; }
        public long? TeamId { get; set; }
        public long? DirectManagerId { get; set; }
        public string EmploymentStatus { get; set; } = string.Empty;
        public string? StaffType { get; set; }
        public int? ProbationMonth { get; set; }
        public string? Shift { get; set; }
        public string? FingerPrintId { get; set; }
        public string? ProductProject { get; set; }
        public bool MobileAttendance { get; set; }
        public DateOnly? DateOfAppointment { get; set; }
        public DateOnly? DateOfConfirmation { get; set; }
        public DateOnly? DateOfPromotion { get; set; }

        public DateTime? DateOfAppointmentProxy
        {
            get => DateOfAppointment?.ToDateTime(TimeOnly.MinValue);
            set => DateOfAppointment = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }
        public DateTime? DateOfConfirmationProxy
        {
            get => DateOfConfirmation?.ToDateTime(TimeOnly.MinValue);
            set => DateOfConfirmation = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }
        public DateTime? DateOfPromotionProxy
        {
            get => DateOfPromotion?.ToDateTime(TimeOnly.MinValue);
            set => DateOfPromotion = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

        // ── Contact ──
        public string? ContactAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? PhoneNo { get; set; }
        public string? PermanentPhoneNo { get; set; }
        public string? PresentPhoneNo { get; set; }
        public string? InternalPhoneNo { get; set; }
        public string? EmergencyMobileNo { get; set; }
        public string? RelationWithEmergencyContact { get; set; }

        // ── Family ──
        public string? MaritalStatus { get; set; }
        public string? SpouseName { get; set; }
        public string? SpouseNRCNo { get; set; }
        public string? SpouseOccupation { get; set; }
        public string? FatherName { get; set; }
        public string? FatherNRCNo { get; set; }
        public string? FatherOccupation { get; set; }

        // ── Payroll ──
        public decimal Salary { get; set; }
        public string? Currency { get; set; }
        public string? PayType { get; set; }
        public string? CostAllocate { get; set; }
        public string? PayByBacklog { get; set; }
        public string? TaxStatus { get; set; }
        public string? TaxNo { get; set; }
        public string? SSBStatus { get; set; }
        public string? SSCBNo { get; set; }
        public int? ComplianceEarnedPoints { get; set; }
        public int? ComplianceBalancePoints { get; set; }
    }
}
