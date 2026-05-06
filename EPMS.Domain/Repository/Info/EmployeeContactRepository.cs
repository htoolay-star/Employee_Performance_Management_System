using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeeContactRepository : GenericRepository<EmployeeContact>, IEmployeeContactRepository
    {
        public EmployeeContactRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeContact>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(h => h.EmployeeId == employeeId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }
    }
}
