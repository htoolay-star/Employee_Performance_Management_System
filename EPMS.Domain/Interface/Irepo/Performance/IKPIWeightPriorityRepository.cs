using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IKPIWeightPriorityRepository : IGenericRepository<KPIWeightPriority>
{
    Task<IEnumerable<KPIWeightPriority>> GetActiveAsync();
    Task<KPIWeightPriority?> GetByLevelNameAsync(string levelName);
    Task<bool> LevelNameExistsAsync(string levelName);
    Task<bool> LevelNameExistsAsync(string levelName, long excludeId);
}
