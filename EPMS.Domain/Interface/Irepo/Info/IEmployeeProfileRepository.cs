using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IEmployeeProfileRepository : IGenericRepository<EmployeeProfile>
    {
        Task<EmployeeProfile?> GetByStaffNoAsync(string code);
        Task<EmployeeProfile?> GetByUserIdAsync(long userId);
    }
}
