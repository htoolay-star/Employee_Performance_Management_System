namespace EPMS.Shared.DTOs.DepartmentDTOs
{
    public record UpdateDepartmentDto
    {
        public string? Description { get; init; }
        public long? DeptHeadId { get; init; }
        public bool IsActive { get; init; } = true;
    }
}
