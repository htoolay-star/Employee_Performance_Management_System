namespace EPMS.Client.Models.Info
{
    public class EmployeeContactFormModel
    {
        public long Id { get; set; }
        public string? ContactAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? PhoneNo { get; set; }
        public string? PermanentPhoneNo { get; set; }
        public string? PresentPhoneNo { get; set; }
        public string? InternalPhoneNo { get; set; }
        public string? EmergencyMobileNo { get; set; }
        public string? RelationWithEmergencyContact { get; set; }
    }
}