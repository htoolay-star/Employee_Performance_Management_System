namespace EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs
{
    public class CreatePIPDto
    {
        public long EmployeeId { get; set; }
        public long ManagerId { get; set; }
        public long? AppraisalId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}