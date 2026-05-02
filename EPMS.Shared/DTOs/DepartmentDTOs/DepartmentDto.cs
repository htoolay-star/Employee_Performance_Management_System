using EPMS.Shared.DTOs.TeamDTOs;

namespace EPMS.Shared.DTOs.DepartmentDTOs
{
    public record DepartmentDto
    {
        public long Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; }

        public List<TeamDto> Teams { get; init; } = new();
    }
}
