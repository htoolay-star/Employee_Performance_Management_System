using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class PositionKPIRepository : GenericRepository<PositionKPI>, IPositionKPIRepository
    {
        public PositionKPIRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<PositionKPI>> GetByPositionIdAsync(long positionId)
            => await _dbSet
                .Include(p => p.KPI)
                .Include(p => p.Priority)
                .Where(p => p.PositionId == positionId)
                .ToListAsync();

        public async Task<bool> ExistsAsync(long positionId, long kpiId, long? excludeId = null)
        {
            var query = _dbSet.Where(p => p.PositionId == positionId && p.KPIId == kpiId);
            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}