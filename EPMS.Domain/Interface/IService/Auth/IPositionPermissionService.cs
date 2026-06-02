using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.Auth;

public interface IPositionPermissionService
{
    Task<SuccessResponse> UpdatePositionPermissionsAsync(long positionId, List<long> selectedPermissionIds);
    Task<SuccessResponse> GetByIdAsync(long id);
    Task<SuccessResponse> GetAllAsync();
    Task<SuccessResponse> GetByPositionIdAsync(long positionId);
    Task<SuccessResponse> GetByPermissionIdAsync(long permissionId);
    Task<SuccessResponse> GetPermissionsForPositionAsync(long positionId);
}
