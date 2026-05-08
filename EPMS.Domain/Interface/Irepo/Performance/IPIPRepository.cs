using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IPIPRepository : IGenericRepository<PIP>
    {
        Task<IEnumerable<PIP>> GetByEmployeeIdAsync(long employeeId);
        Task<IEnumerable<PIP>> GetByManagerIdAsync(long managerId);
        Task<IEnumerable<PIP>> GetActivePIPsAsync();
    }
}