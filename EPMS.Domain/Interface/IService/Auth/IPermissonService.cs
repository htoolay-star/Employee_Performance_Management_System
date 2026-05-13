using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.Auth;

public interface IPermissionService
{
    Task<SuccessResponse<IEnumerable<PermissionDto>>> GetAllPermissionsAsync();
    Task<SuccessResponse<PermissionDto>> GetPermissionByIdAsync(long id);
}
