using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeeEmploymentHistoryRepository : GenericRepository<EmployeeEmploymentHistory>, IEmployeeEmploymentHistoryRepository
    {
        public EmployeeEmploymentHistoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeEmploymentHistory>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(h => h.EmployeeId == employeeId)
                .OrderByDescending(h => h.EffectiveDate)
                .ToListAsync();
        }
    }
}
