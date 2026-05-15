using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IEntityKPIHistoryRepository : IGenericRepository<EntityKPIHistory>
    {
        Task<IEnumerable<EntityKPIHistory>> GetByCycleAsync(long cycleId);
        Task<IEnumerable<EntityKPIHistory>> GetByEntityAndCycleAsync(string entityType, long entityId, long cycleId);
    }
}
