using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IEmployeeContactRepository : IGenericRepository<EmployeeContact>
    {
        Task<IEnumerable<EmployeeContact>> GetByEmployeeIdAsync(long employeeId);
    }
}
