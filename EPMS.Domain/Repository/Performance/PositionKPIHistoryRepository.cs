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
    public class PositionKPIHistoryRepository : GenericRepository<PositionKPIHistory>, IPositionKPIHistoryRepository
    {
        public PositionKPIHistoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<PositionKPIHistory>> GetHistoryByPositionAsync(long positionId)
        {
            return await _dbSet
                .Include(h => h.KPI)
                .Include(h => h.Priority)
                .Include(h => h.ChangedBy)
                .Where(h => h.PositionId == positionId)
                .OrderByDescending(h => h.EffectiveDate)
                .ToListAsync();
        }

        public async Task<PositionKPIHistory?> GetLatestHistoryAsync(long positionId, long kpiId)
        {
            return await _dbSet
                .Where(h => h.PositionId == positionId && h.KPIId == kpiId && h.EndDate == null)
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
