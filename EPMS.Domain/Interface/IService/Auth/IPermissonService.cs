using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.Auth;

public interface IPermissionService
{
    Task<SuccessResponse<IEnumerable<PermissionDto>>> GetAllPermissionsAsync();
    Task<SuccessResponse<PermissionDto>> GetPermissionByIdAsync(long id);
    Task<SuccessResponse<long>> CreatePermissionAsync(CreatePermissionDto dto);
    Task<SuccessResponse> UpdatePermissionAsync(long id, UpdatePermissionDto dto);
    Task<SuccessResponse> DeletePermissionAsync(long id);
}
