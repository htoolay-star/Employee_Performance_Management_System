using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;

namespace EPMS.Domain.Interface.Irepo.Auth;

public interface IPositionPermissionRepository : IGenericRepository<PositionPermission>
{
    Task<IEnumerable<PositionPermission>> GetByPositionIdAsync(long positionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PositionPermission>> GetByPermissionIdAsync(long permissionId, CancellationToken cancellationToken = default);
    Task<PositionPermission?> GetByPositionAndPermissionAsync(long positionId, long permissionId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long positionId, long permissionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetPermissionsForPositionAsync(long positionId, CancellationToken cancellationToken = default);
}
