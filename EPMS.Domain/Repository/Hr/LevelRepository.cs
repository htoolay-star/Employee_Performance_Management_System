using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Hr;

public class LevelRepository : GenericRepository<Level>, ILevelRepository
{
    public LevelRepository(AppDbContext context) : base(context) { }

    public async Task<bool> ExistsByCodeAsync(string code, long? excludeLevelId = null, CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim().ToUpperInvariant();
        var query = _dbSet.Where(l => l.Code == normalized);
        if (excludeLevelId.HasValue)
            query = query.Where(l => l.Id != excludeLevelId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, long? excludeLevelId = null, CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToUpperInvariant();
        var query = _dbSet.Where(l => l.Name == normalized);
        if (excludeLevelId.HasValue)
            query = query.Where(l => l.Id != excludeLevelId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<(long Id, string Code, bool IsActive)>> GetLookupAsync()
    {
        return await _dbSet
            .Select(x => new ValueTuple<long, string, bool>(x.Id, x.Code, x.IsActive))
            .ToListAsync();
    }
}
