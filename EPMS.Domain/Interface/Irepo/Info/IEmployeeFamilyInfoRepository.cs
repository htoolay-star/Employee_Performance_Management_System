using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IEmployeeFamilyInfoRepository : IGenericRepository<EmployeeFamilyInfo>
    {
        Task<IEnumerable<EmployeeFamilyInfo>> GetByEmployeeIdAsync(long employeeId);
    }
}
