using EPMS.Domain.Contracts;
using EPMS.Domain.Interface.IService.Auth;

namespace EPMS.Domain.Services.Auth;

public class PositionPermissionChecker : IPositionPermissionChecker
{
    private readonly IUnitOfWork _uow;

    public PositionPermissionChecker(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> HasPermissionAsync(long positionId, string permissionCode, CancellationToken cancellationToken = default)
    {
        if (positionId <= 0) return false;
        if (string.IsNullOrWhiteSpace(permissionCode)) return false;

        var permission = await _uow.Auth.Permissions.GetByCodeAsync(permissionCode);

        if (permission == null) return false;

        return await _uow.Auth.PositionPermissions.ExistsAsync(positionId, permission.Id, cancellationToken);
    }
}
