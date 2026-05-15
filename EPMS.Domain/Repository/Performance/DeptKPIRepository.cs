using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class DeptKPIRepository : GenericRepository<DeptKPI>, IDeptKPIRepository
    {
        public DeptKPIRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<DeptKPI>> GetByDeptIdAsync(long deptId)
            => await _dbSet
                .Include(p => p.KPI)
                .Include(p => p.Priority)
                .Where(p => p.DeptId == deptId)
                .ToListAsync();

        public async Task<bool> ExistsAsync(long deptId, long kpiId, long? excludeId = null)
        {
            var query = _dbSet.Where(p => p.DeptId == deptId && p.KPIId == kpiId);
            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}
