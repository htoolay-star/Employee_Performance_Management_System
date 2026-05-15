using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class EmployeeKPIRepository : GenericRepository<EmployeeKPI>, IEmployeeKPIRepository
    {
        public EmployeeKPIRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeKPI>> GetByEmployeeAndCycleAsync(long employeeId, long cycleId)
            => await _dbSet
                .Include(e => e.KPI)
                .Include(e => e.Priority)
                .Where(e => e.EmployeeId == employeeId && e.CycleId == cycleId)
                .ToListAsync();

        public async Task<IEnumerable<EmployeeKPI>> GetByCycleAsync(long cycleId)
            => await _dbSet
                .Include(e => e.KPI)
                .Include(e => e.Priority)
                .Where(e => e.CycleId == cycleId)
                .ToListAsync();

        public async Task<bool> ExistsAsync(long employeeId, long kpiId, long cycleId, long? excludeId = null)
        {
            var query = _dbSet.Where(e => e.EmployeeId == employeeId && e.KPIId == kpiId && e.CycleId == cycleId);
            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}
