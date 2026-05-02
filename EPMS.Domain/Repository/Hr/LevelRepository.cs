using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Hr;

public class LevelRepository : GenericRepository<Level>, ILevelRepository
{
    public LevelRepository(AppDbContext context) : base(context) { }

    public async Task<bool> ExistsByCodeAsync(string code, int? excludeLevelId = null, CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim().ToUpperInvariant();
        var query = _dbSet.Where(l => l.Code == normalized);
        if (excludeLevelId.HasValue)
            query = query.Where(l => l.Id != excludeLevelId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> HasPositionsAsync(int levelId, CancellationToken cancellationToken = default)
    {
        return await _context.Positions.AnyAsync(p => p.LevelId == levelId, cancellationToken);
    }
}
