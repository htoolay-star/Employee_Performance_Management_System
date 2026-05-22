using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;
using EPMS.Shared.DTOs.Common;
using Refit;

namespace EPMS.Client.Services.Auth;

public interface IPermissionApiClient
{
    [Get("/api/permissions")]
    Task<SuccessResponse<IEnumerable<PermissionDto>>> GetAllPermissions();

    [Get("/api/position-permissions/position/{positionId}/permissions")]
    Task<SuccessResponse<IEnumerable<PermissionDto>>> GetPermissionsForPosition(long positionId);

    [Put("/api/position-permissions/position/{positionId}")]
    Task<SuccessResponse> UpdatePositionPermissions(long positionId, [Body] List<long> permissionIds);
}
