namespace EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;

public record PositionPermissionDto(
    long Id,
    long PositionId,
    long PermissionId,
    string? PositionTitle,
    string? PermissionName,
    string? PermissionCode);
