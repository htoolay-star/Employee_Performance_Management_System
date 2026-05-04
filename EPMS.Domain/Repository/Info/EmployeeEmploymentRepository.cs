using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeeEmploymentRepository : GenericRepository<EmployeeEmployment>, IEmployeeEmploymentRepository
    {
        public EmployeeEmploymentRepository(AppDbContext context) : base(context) { }

        public async Task<EmployeeEmployment?> GetByEmployeeIdAsync(long employeeId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Team)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }
    }
}
