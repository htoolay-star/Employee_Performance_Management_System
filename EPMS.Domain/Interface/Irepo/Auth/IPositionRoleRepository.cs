using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Auth;

public interface IPositionRoleRepository : IGenericRepository<PositionRole>
{
    Task<IEnumerable<PositionRole>> GetByPositionIdAsync(long positionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PositionRole>> GetByRoleIdAsync(long roleId, CancellationToken cancellationToken = default);
    Task<PositionRole?> GetByPositionAndRoleAsync(long positionId, long roleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long positionId, long roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetRolesForPositionAsync(long positionId, CancellationToken cancellationToken = default);
}