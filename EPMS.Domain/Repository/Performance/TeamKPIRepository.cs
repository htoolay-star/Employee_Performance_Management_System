using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class TeamKPIRepository : GenericRepository<TeamKPI>, ITeamKPIRepository
    {
        public TeamKPIRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<TeamKPI>> GetByTeamIdAsync(long teamId)
            => await _dbSet
                .Include(p => p.KPI)
                .Include(p => p.Priority)
                .Where(p => p.TeamId == teamId)
                .ToListAsync();

        public async Task<bool> ExistsAsync(long teamId, long kpiId, long? excludeId = null)
        {
            var query = _dbSet.Where(p => p.TeamId == teamId && p.KPIId == kpiId);
            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}
