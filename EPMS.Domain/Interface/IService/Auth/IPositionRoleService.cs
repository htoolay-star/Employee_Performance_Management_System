using EPMS.Shared.DTOs.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.Auth;

public interface IPositionRoleService
{
    Task<SuccessResponse<IEnumerable<long>>> GetAdminPositionIdsAsync();
    Task<SuccessResponse> ToggleAdminRoleAsync(long positionId, bool isGrantingAdmin);
}