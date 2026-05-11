namespace EPMS.Shared.DTOs.DepartmentDTOs
{
    public record CreateDepartmentDto
    {
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public long? DeptHeadId { get; init; }
    }
}
