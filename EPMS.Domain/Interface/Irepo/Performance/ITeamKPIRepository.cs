using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface ITeamKPIRepository : IGenericRepository<TeamKPI>
    {
        Task<IEnumerable<TeamKPI>> GetByTeamIdAsync(long teamId);
        Task<bool> ExistsAsync(long teamId, long kpiId, long? excludeId = null);
    }
}
