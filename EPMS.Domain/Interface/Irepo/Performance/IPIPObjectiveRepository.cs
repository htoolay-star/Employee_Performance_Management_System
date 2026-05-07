using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IPIPObjectiveRepository : IGenericRepository<PIPObjective>
{
    Task<IEnumerable<PIPObjective>> GetByPIPIdAsync(long pipId);
}