using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Auth;

public class PositionPermissionRepository : GenericRepository<PositionPermission>, IPositionPermissionRepository
{
    public PositionPermissionRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PositionPermission>> GetByPositionIdAsync(long positionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(pp => pp.PositionId == positionId)
            .Include(pp => pp.Permission)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PositionPermission>> GetByPermissionIdAsync(long permissionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(pp => pp.PermissionId == permissionId)
            .Include(pp => pp.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<PositionPermission?> GetByPositionAndPermissionAsync(long positionId, long permissionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(pp => pp.PositionId == positionId && pp.PermissionId == permissionId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(long positionId, long permissionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(pp => pp.PositionId == positionId && pp.PermissionId == permissionId, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetPermissionsForPositionAsync(long positionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(pp => pp.PositionId == positionId)
            .Select(pp => pp.Permission)
            .ToListAsync(cancellationToken);
    }
}
