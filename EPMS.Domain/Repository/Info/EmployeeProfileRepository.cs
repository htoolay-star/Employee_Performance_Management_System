using EPMS.Domain.Data;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.Irepo.Info;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Info
{
    public class EmployeeProfileRepository : GenericRepository<EmployeeProfile>, IEmployeeProfileRepository
    {
        public EmployeeProfileRepository(AppDbContext context) : base(context) { }

        public async Task<EmployeeProfile?> GetByStaffNoAsync(string staffNo) =>
            await _dbSet.FirstOrDefaultAsync(p => p.StaffNo == staffNo);

        public async Task<EmployeeProfile?> GetByUserIdAsync(long userId) =>
            await _dbSet.FirstOrDefaultAsync(p => p.UserId == userId);
    }
}
