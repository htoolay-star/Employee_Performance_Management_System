using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IEmployeeEmploymentRepository : IGenericRepository<EmployeeEmployment>
    {
        Task<EmployeeEmployment?> GetByEmployeeIdAsync(long employeeId);
    }
}
