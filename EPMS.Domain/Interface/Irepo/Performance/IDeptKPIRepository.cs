using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IDeptKPIRepository : IGenericRepository<DeptKPI>
    {
        Task<IEnumerable<DeptKPI>> GetByDeptIdAsync(long deptId);
        Task<bool> ExistsAsync(long deptId, long kpiId, long? excludeId = null);
    }
}
