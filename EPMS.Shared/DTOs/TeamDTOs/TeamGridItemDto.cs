namespace EPMS.Shared.DTOs.TeamDTOs
{
    public record TeamGridItemDto : TeamDto
    {
        public int RowIndex { get; init; }
        public bool DepartmentIsActive { get; init; }
    }
}