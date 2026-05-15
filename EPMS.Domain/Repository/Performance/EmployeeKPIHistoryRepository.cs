using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class EmployeeKPIHistoryRepository : GenericRepository<EmployeeKPIHistory>, IEmployeeKPIHistoryRepository
    {
        public EmployeeKPIHistoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeKPIHistory>> GetByCycleAsync(long cycleId)
            => await _dbSet
                .Include(h => h.KPI)
                .Include(h => h.Priority)
                .Where(h => h.CycleId == cycleId)
                .ToListAsync();

        public async Task<IEnumerable<EmployeeKPIHistory>> GetByEmployeeAndCycleAsync(long employeeId, long cycleId)
            => await _dbSet
                .Include(h => h.KPI)
                .Include(h => h.Priority)
                .Where(h => h.EmployeeId == employeeId && h.CycleId == cycleId)
                .ToListAsync();
    }
}
