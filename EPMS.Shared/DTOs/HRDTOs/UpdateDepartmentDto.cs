namespace EPMS.Shared.DTOs.HR
{
    public record UpdateDepartmentDto
    {
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;
    }
}
