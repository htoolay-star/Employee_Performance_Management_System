namespace EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;

public record CreatePositionPermissionDto
{
    public long PositionId { get; init; }
    public long PermissionId { get; init; }
}
