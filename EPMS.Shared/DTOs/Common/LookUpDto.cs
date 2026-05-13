namespace EPMS.Shared.DTOs.Common
{
    public record LookUpDto
    {
        public long Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}