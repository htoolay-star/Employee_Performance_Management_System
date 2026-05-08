namespace EPMS.Shared.DTOs.PerformanceDTOs.KPIMasterDTOs
{
    public class CreateKPIMasterDto
    {
        public long CategoryId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}