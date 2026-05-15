using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IEmployeeKPIHistoryRepository : IGenericRepository<EmployeeKPIHistory>
    {
        Task<IEnumerable<EmployeeKPIHistory>> GetByCycleAsync(long cycleId);
        Task<IEnumerable<EmployeeKPIHistory>> GetByEmployeeAndCycleAsync(long employeeId, long cycleId);
    }
}
