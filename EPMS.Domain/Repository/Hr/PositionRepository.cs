using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Hr;

public class PositionRepository : GenericRepository<Position>, IPositionRepository
{
    public PositionRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Position>> GetAllWithLevelAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Include(p => p.Level)
            .ToListAsync(cancellationToken);
    }

    public async Task<Position?> GetByIdWithLevelAsync(long id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Position> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
        return await query
            .Include(p => p.Level)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByTitleAsync(string title, long? excludePositionId = null, CancellationToken cancellationToken = default)
    {
        var normalized = title.Trim();
        var query = _dbSet.Where(p => p.Title == normalized);
        if (excludePositionId.HasValue)
            query = query.Where(p => p.Id != excludePositionId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> LevelExistsAsync(int levelId, CancellationToken cancellationToken = default)
    {
        return await _context.Levels.AnyAsync(l => l.Id == levelId, cancellationToken);
    }
}
