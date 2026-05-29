namespace EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs
{
    public class PIPDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public long ManagerId { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public long? AppraisalId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? FinalOutcomeNotes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsCurrentUserEmployee { get; set; }
        public int TotalObjectives { get; set; }
        public int CompletedObjectives { get; set; }
    }
}