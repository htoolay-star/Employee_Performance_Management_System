using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IEmployeePayrollInfoRepository : IGenericRepository<EmployeePayrollInfo>
    {
        Task<EmployeePayrollInfo?> GetByEmployeeIdAsync(long employeeId);
    }
}
