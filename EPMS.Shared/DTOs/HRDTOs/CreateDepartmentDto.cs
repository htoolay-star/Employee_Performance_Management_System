namespace EPMS.Shared.DTOs.HR
{
    public record CreateDepartmentDto
    {
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}
