namespace EPMS.Client.Models.Info
{
    public class EmployeeProfileFormModel
    {
        public long Id { get; set; }
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
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? PassportExpireDate { get; set; }
        public string? WorkPermitNo { get; set; }
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
    }
}