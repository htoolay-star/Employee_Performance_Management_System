using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class KPIWeightPriorityRepository : GenericRepository<KPIWeightPriority>, IKPIWeightPriorityRepository
{
    public KPIWeightPriorityRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<KPIWeightPriority>> GetActiveAsync()
    {
        return await _dbSet
            .Where(kpi => kpi.IsActive && !kpi.IsDeleted)
            .OrderBy(kpi => kpi.MinWeight)
            .ToListAsync();
    }

    public async Task<KPIWeightPriority?> GetByLevelNameAsync(string levelName)
    {
        return await _dbSet
            .FirstOrDefaultAsync(kpi => kpi.LevelName == levelName && !kpi.IsDeleted);
    }

    public async Task<bool> LevelNameExistsAsync(string levelName)
    {
        return await _dbSet.AnyAsync(kpi => kpi.LevelName == levelName && !kpi.IsDeleted);
    }

    public async Task<bool> LevelNameExistsAsync(string levelName, long excludeId)
    {
        return await _dbSet.AnyAsync(kpi => kpi.LevelName == levelName && kpi.Id != excludeId && !kpi.IsDeleted);
    }
}
