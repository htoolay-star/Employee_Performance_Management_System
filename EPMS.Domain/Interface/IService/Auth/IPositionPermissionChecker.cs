namespace EPMS.Domain.Interface.IService.Auth;

public interface IPositionPermissionChecker
{
    Task<bool> HasPermissionAsync(long positionId, string permissionCode, CancellationToken cancellationToken = default);
}
