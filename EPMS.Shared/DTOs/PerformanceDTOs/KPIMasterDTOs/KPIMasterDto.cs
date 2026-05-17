using EPMS.Shared.Constants;

namespace EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs
{
    public class KPIMasterDto
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ScoringDirection { get; set; } = AppraisalConstants.ScoringDirections.HigherIsBetter;
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}