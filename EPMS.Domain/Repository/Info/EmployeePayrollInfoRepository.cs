using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeePayrollInfoRepository : GenericRepository<EmployeePayrollInfo>, IEmployeePayrollInfoRepository
    {
        public EmployeePayrollInfoRepository(AppDbContext context) : base(context) { }

        public async Task<EmployeePayrollInfo?> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.EmployeeId == employeeId);
        }
    }
}
