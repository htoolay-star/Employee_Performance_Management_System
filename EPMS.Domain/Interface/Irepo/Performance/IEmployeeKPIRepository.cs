using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IEmployeeKPIRepository : IGenericRepository<EmployeeKPI>
    {
        Task<IEnumerable<EmployeeKPI>> GetByEmployeeAndCycleAsync(long employeeId, long cycleId);
        Task<IEnumerable<EmployeeKPI>> GetByCycleAsync(long cycleId);
        Task<bool> ExistsAsync(long employeeId, long kpiId, long cycleId, long? excludeId = null);
    }
}
