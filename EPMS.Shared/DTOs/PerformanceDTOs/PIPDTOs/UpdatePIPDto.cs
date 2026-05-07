namespace EPMS.Shared.DTOs.PerformanceDTOs.PIPDTOs
{
    public class UpdatePIPDto
    {
        public long Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}