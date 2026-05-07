using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IPositionKPIRepository : IGenericRepository<PositionKPI>
    {
        Task<IEnumerable<PositionKPI>> GetByPositionIdAsync(long positionId);
        Task<bool> ExistsAsync(long positionId, long kpiId, long? excludeId = null);
    }
}