using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class EntityKPIRepository : GenericRepository<EntityKPI>, IEntityKPIRepository
    {
        public EntityKPIRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EntityKPI>> GetAllWithIncludesAsync()
            => await _dbSet
                .Include(e => e.KPI)
                .Include(e => e.Priority)
                .ToListAsync();

        public async Task<IEnumerable<EntityKPI>> GetByEntityAsync(string entityType, long entityId)
            => await _dbSet
                .Include(e => e.KPI)
                .Include(e => e.Priority)
                .Where(e => e.EntityType == entityType && e.EntityId == entityId)
                .ToListAsync();

        public async Task<IEnumerable<EntityKPI>> GetByEntityTypeAsync(string entityType)
            => await _dbSet
                .Include(e => e.KPI)
                .Include(e => e.Priority)
                .Where(e => e.EntityType == entityType)
                .ToListAsync();

        public async Task<bool> ExistsAsync(string entityType, long entityId, long kpiId, long? excludeId = null)
        {
            var query = _dbSet.Where(e => e.EntityType == entityType && e.EntityId == entityId && e.KPIId == kpiId);
            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<decimal> GetTotalWeightageAsync(string entityType, long entityId, long? excludeId = null)
        {
            var query = _dbSet.Where(e => e.EntityType == entityType && e.EntityId == entityId);
            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);
            return await query.SumAsync(e => (decimal?)e.Weightage) ?? 0;
        }
    }
}
