using EPMS.Shared.DTOs.AuthDTOs.PositionPermissionDTOs;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Services.Auth;

public interface IPositionPermissionService
{
    Task<SuccessResponse> CreateAsync(CreatePositionPermissionDto dto);
    Task<SuccessResponse> DeleteAsync(long positionId, long permissionId);
    Task<SuccessResponse> GetByIdAsync(long id);
    Task<SuccessResponse> GetAllAsync();
    Task<SuccessResponse> GetByPositionIdAsync(long positionId);
    Task<SuccessResponse> GetByPermissionIdAsync(long permissionId);
    Task<SuccessResponse> GetPermissionsForPositionAsync(long positionId);
}
