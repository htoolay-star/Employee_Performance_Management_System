using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Performance
{
    public class PositionKPIRepository : GenericRepository<PositionKPI>, IPositionKPIRepository
    {
        public PositionKPIRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<PositionKPI>> GetByPositionIdAsync(long positionId)
        {
            return await _dbSet
                .Include(pk => pk.KPI)
                .Include(pk => pk.Priority)
                .Where(pk => pk.PositionId == positionId && !pk.IsDeleted)
                .ToListAsync();
        }

        public async Task<PositionKPI?> GetByPositionAndKPIAsync(long positionId, long kpiId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(pk => pk.PositionId == positionId && pk.KPIId == kpiId && !pk.IsDeleted);
        }

        public async Task<decimal> GetTotalWeightageByPositionAsync(long positionId, long? excludeKPIId = null)
        {
            var query = _dbSet.Where(pk => pk.PositionId == positionId && !pk.IsDeleted);

            if (excludeKPIId.HasValue)
                query = query.Where(pk => pk.KPIId != excludeKPIId.Value);

            return await query.SumAsync(pk => pk.Weightage);
        }
    }
}
