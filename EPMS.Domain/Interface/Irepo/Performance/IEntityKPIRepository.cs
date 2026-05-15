using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IEntityKPIRepository : IGenericRepository<EntityKPI>
    {
        Task<IEnumerable<EntityKPI>> GetByEntityAsync(string entityType, long entityId);
        Task<IEnumerable<EntityKPI>> GetByEntityTypeAsync(string entityType);
        Task<bool> ExistsAsync(string entityType, long entityId, long kpiId, long? excludeId = null);
    }
}
