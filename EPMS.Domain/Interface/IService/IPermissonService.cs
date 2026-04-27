using EPMS.Shared.DTOs.PermissionDTOS;

namespace EPMS.Domain.Interface.IService
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        Task<PermissionDto?> GetPermissionByIdAsync(int id);
        Task CreatePermissionAsync(CreatePermissionDto dto);
        Task UpdatePermissionAsync(int id, UpdatePermissionDto dto);
        Task DeletePermissionAsync(int id);
    }
}
