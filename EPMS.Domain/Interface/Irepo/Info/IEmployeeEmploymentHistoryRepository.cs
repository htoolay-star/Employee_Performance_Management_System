using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IEmployeeEmploymentHistoryRepository : IGenericRepository<EmployeeEmploymentHistory>
    {
        Task<IEnumerable<EmployeeEmploymentHistory>> GetByEmployeeIdAsync(long employeeId);
    }
}
