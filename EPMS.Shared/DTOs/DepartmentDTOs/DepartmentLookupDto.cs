namespace EPMS.Shared.DTOs.DepartmentDTOs
{
    public record DepartmentLookupDto
    {
        public long Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}