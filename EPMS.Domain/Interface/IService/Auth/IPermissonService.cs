using EPMS.Shared.DTOs.AuthDTOs.PermissionDTOS;
using EPMS.Shared.DTOs.Common;

namespace EPMS.Domain.Interface.IService.Auth
{
    public interface IPermissionService
    {
        Task<SuccessResponse<IEnumerable<PermissionDto>>> GetAllPermissionsAsync();
        Task<SuccessResponse<PermissionDto>> GetPermissionByIdAsync(int id);
        Task<SuccessResponse> CreatePermissionAsync(CreatePermissionDto dto);
        Task<SuccessResponse> UpdatePermissionAsync(int id, UpdatePermissionDto dto);
        Task<SuccessResponse> DeletePermissionAsync(int id);
    }
}
