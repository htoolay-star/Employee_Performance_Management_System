using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeeFamilyInfoRepository : GenericRepository<EmployeeFamilyInfo>, IEmployeeFamilyInfoRepository
    {
        public EmployeeFamilyInfoRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<EmployeeFamilyInfo>> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(f => f.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
