using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class EntityKPIHistoryRepository : GenericRepository<EntityKPIHistory>, IEntityKPIHistoryRepository
    {
        public EntityKPIHistoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EntityKPIHistory>> GetByCycleAsync(long cycleId)
            => await _dbSet
                .Include(h => h.KPI)
                .Include(h => h.Priority)
                .Where(h => h.CycleId == cycleId)
                .OrderBy(h => h.EntityType).ThenBy(h => h.EntityId)
                .ToListAsync();

        public async Task<IEnumerable<EntityKPIHistory>> GetByEntityAndCycleAsync(string entityType, long entityId, long cycleId)
            => await _dbSet
                .Include(h => h.KPI)
                .Include(h => h.Priority)
                .Where(h => h.EntityType == entityType && h.EntityId == entityId && h.CycleId == cycleId)
                .ToListAsync();
    }
}
