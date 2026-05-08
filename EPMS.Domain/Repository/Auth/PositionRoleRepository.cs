using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Auth;

public class PositionRoleRepository : GenericRepository<PositionRole>, IPositionRoleRepository
{
    public PositionRoleRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PositionRole>> GetByPositionIdAsync(long positionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(pr => pr.PositionId == positionId)
            .Include(pr => pr.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PositionRole>> GetByRoleIdAsync(long roleId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(pr => pr.RoleId == roleId)
            .Include(pr => pr.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<PositionRole?> GetByPositionAndRoleAsync(long positionId, long roleId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(pr => pr.PositionId == positionId && pr.RoleId == roleId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(long positionId, long roleId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(pr => pr.PositionId == positionId && pr.RoleId == roleId, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetRolesForPositionAsync(long positionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Where(pr => pr.PositionId == positionId)
            .Select(pr => pr.Role)
            .ToListAsync(cancellationToken);
    }
}