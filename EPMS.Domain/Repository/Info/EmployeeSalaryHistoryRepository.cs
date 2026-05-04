using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeeSalaryHistoryRepository : GenericRepository<EmployeeSalaryHistory>, IEmployeeSalaryHistoryRepository
    {
        public EmployeeSalaryHistoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeSalaryHistory>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(h => h.EmployeeId == employeeId)
                .OrderByDescending(h => h.EffectiveDate)
                .ToListAsync();
        }
    }
}
