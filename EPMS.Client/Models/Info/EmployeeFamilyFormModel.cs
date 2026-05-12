namespace EPMS.Client.Models.Info
{
    public class EmployeeFamilyFormModel
    {
        public long Id { get; set; }
        public string? MaritalStatus { get; set; }
        public string? SpouseName { get; set; }
        public string? SpouseNRCNo { get; set; }
        public string? SpouseOccupation { get; set; }
        public string? FatherName { get; set; }
        public string? FatherNRCNo { get; set; }
        public string? FatherOccupation { get; set; }
    }
}