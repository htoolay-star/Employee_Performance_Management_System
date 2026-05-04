using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IEmployeeSalaryHistoryRepository : IGenericRepository<EmployeeSalaryHistory>
    {
        Task<IEnumerable<EmployeeSalaryHistory>> GetByEmployeeIdAsync(long employeeId);
    }
}
