using EPMS.Shared.DTOs.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Auth;

public interface IPositionRoleService
{
    Task<SuccessResponse<IEnumerable<PositionRoleDto>>> GetRolesForPositionAsync(long positionId);
    Task<SuccessResponse<long>> AssignRoleToPositionAsync(long positionId, long roleId);
    Task<SuccessResponse> RemoveRoleFromPositionAsync(long positionId, long roleId);
    Task<SuccessResponse<IEnumerable<PositionRoleDto>>> GetPositionsForRoleAsync(long roleId);
}

public class PositionRoleDto
{
    public long Id { get; set; }
    public long PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public long RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}