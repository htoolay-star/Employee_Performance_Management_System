namespace EPMS.Shared.Validators.ValidationMessages;

public static class EmployeeInfoValidationMessages
{
    public static class EmployeeProfile
    {
        public const string StaffNoRequired = "Staff number is required.";
        public const string StaffNoMaxLength = "Staff number cannot exceed 20 characters.";
        public const string StaffNameRequired = "Staff name is required.";
        public const string StaffNameMaxLength = "Staff name cannot exceed 100 characters.";
        public const string OtherNameMaxLength = "Other name cannot exceed 100 characters.";
        public const string NRCMaxLength = "NRC number cannot exceed 50 characters.";
        public const string GenderInvalid = "Gender is not valid.";
        public const string ReligionInvalid = "Religion is not valid.";
        public const string DateOfBirthFuture = "Date of birth cannot be in the future.";
        public const string NationalityMaxLength = "Nationality cannot exceed 100 characters.";
        public const string WorkPermitNoMaxLength = "Work permit number cannot exceed 50 characters.";
        public const string WorkPermitValidDateFuture = "Work permit valid date cannot be in the future.";
        public const string WorkPermitExpireDateAfterValid = "Work permit expire date must be after valid date.";
        public const string ProfilePictureUrlInvalid = "Profile picture URL must be a valid URL.";
        public const string ProfileThumbnailUrlInvalid = "Profile thumbnail URL must be a valid URL.";
        public const string AdditionalDataMaxLength = "Additional data cannot exceed 2000 characters.";
        public const string EmailAddressMaxLength = "Email address cannot exceed 100 characters.";
        public const string EmailAddressInvalid = "Email address format is invalid.";
        public const string EmailAddressRequired = "Email address is required.";
    }

    public static class EmployeeEmployment
    {
        public const string EmployeeIdInvalid = "Employee ID must be greater than 0.";
        public const string DepartmentIdInvalid = "Please select a valid department.";
        public const string PositionIdInvalid = "Position ID must be greater than 0.";
        public const string EmploymentStatusRequired = "Employment status is required.";
        public const string EmploymentStatusMaxLength = "Employment status cannot exceed 50 characters.";
        public const string EmploymentStatusInvalid = "Employment status must be one of: Permanent, Probation, Resigned.";
        public const string ParentDepartmentIdInvalid = "Parent department ID must be greater than 0.";
        public const string TeamIdInvalid = "Team ID must be greater than 0.";
        public const string DirectManagerIdInvalid = "Direct manager ID must be greater than 0.";
        public const string StaffTypeMaxLength = "Staff type cannot exceed 50 characters.";
        public const string StaffTypeInvalid = "Staff type is not valid.";
        public const string ProbationMonthInvalid = "Probation months must be greater than or equal to 0.";
        public const string ShiftMaxLength = "Shift cannot exceed 50 characters.";
        public const string ShiftInvalid = "Shift is not valid.";
        public const string DateOfPromotionFuture = "Date of promotion cannot be in the future.";
        public const string FingerPrintIdMaxLength = "Fingerprint ID cannot exceed 50 characters.";
        public const string ProductProjectMaxLength = "Product/project cannot exceed 200 characters.";
        public const string DateOfAppointmentFuture = "Date of appointment cannot be in the future.";
        public const string DateOfConfirmationAfterAppointment = "Date of confirmation must be after date of appointment.";
        public const string DateOfIncrementFuture = "Date of increment cannot be in the future.";
    }

    public static class EmployeeContact
    {
        public const string ContactAddressMaxLength = "Contact address cannot exceed 500 characters.";
        public const string PermanentAddressMaxLength = "Permanent address cannot exceed 500 characters.";
        public const string PhoneNumberMaxLength = "Phone number cannot exceed 20 characters.";
        public const string PhoneNumberInvalid = "Phone number format is invalid.";
        public const string EmailAddressMaxLength = "Email address cannot exceed 100 characters.";
        public const string EmailAddressInvalid = "Email address format is invalid.";
        public const string InternalPhoneNoMaxLength = "Internal phone number cannot exceed 20 characters.";
        public const string EmergencyMobileNoMaxLength = "Emergency mobile number cannot exceed 20 characters.";
        public const string RelationWithEmergencyContactMaxLength = "Relation with emergency contact cannot exceed 50 characters.";
        public const string RelationWithEmergencyContactInvalid = "Relation with emergency contact is not valid.";
    }

    public static class EmployeeFamilyInfo
    {
        public const string MaritalStatusMaxLength = "Marital status cannot exceed 50 characters.";
        public const string MaritalStatusInvalid = "Marital status is not valid.";
        public const string SpouseNameMaxLength = "Spouse name cannot exceed 100 characters.";
        public const string SpouseNRCMaxLength = "Spouse NRC number cannot exceed 50 characters.";
        public const string SpouseOccupationMaxLength = "Spouse occupation cannot exceed 100 characters.";
        public const string FatherNameMaxLength = "Father name cannot exceed 100 characters.";
        public const string FatherNRCMaxLength = "Father NRC number cannot exceed 50 characters.";
        public const string FatherOccupationMaxLength = "Father occupation cannot exceed 100 characters.";
    }

    public static class EmployeePayrollInfo
    {
        public const string SalaryInvalid = "Salary must be greater than or equal to 0.";
        public const string CurrencyMaxLength = "Currency cannot exceed 10 characters.";
        public const string CurrencyInvalid = "Currency is not valid.";
        public const string PayTypeMaxLength = "Pay type cannot exceed 50 characters.";
        public const string PayTypeInvalid = "Pay type is not valid.";
        public const string CostAllocateMaxLength = "Cost allocate cannot exceed 100 characters.";
        public const string PayByBacklogMaxLength = "Pay by backlog cannot exceed 50 characters.";
        public const string TaxStatusMaxLength = "Tax status cannot exceed 50 characters.";
        public const string TaxNoMaxLength = "Tax number cannot exceed 50 characters.";
        public const string SSBStatusMaxLength = "SSB status cannot exceed 50 characters.";
        public const string SSCBNoMaxLength = "SSCB number cannot exceed 50 characters.";
        public const string ComplianceEarnedPointsInvalid = "Compliance earned points must be greater than or equal to 0.";
        public const string ComplianceBalancePointsInvalid = "Compliance balance points must be greater than or equal to 0.";
    }

    public static class EmployeeHistory
    {
        public const string EffectiveDateRequired = "Effective date is required.";
        public const string EffectiveDateFuture = "Effective date cannot be in the future.";
        public const string ChangeReasonMaxLength = "Change reason cannot exceed 500 characters.";
        public const string ChangeReasonRequired = "Change reason is required.";
        public const string ManagerIdInvalid = "Manager ID must be greater than 0.";
        public const string ChangedByIdInvalid = "Changed by ID must be greater than 0.";
        public const string ApprovedByIdInvalid = "Approved by ID must be greater than 0.";
        public const string PreviousAmountInvalid = "Previous amount must be greater than or equal to 0.";
        public const string NewAmountInvalid = "New amount must be greater than or equal to 0.";
        public const string NewAmountDifferent = "New amount must be different from previous amount.";
    }

    public static class Common
    {
        public const string NameRequired = "Name is required.";
        public const string NameMaxLength = "Name cannot exceed 100 characters.";
        public const string DateFuture = "Date cannot be in the future.";
    }
}
