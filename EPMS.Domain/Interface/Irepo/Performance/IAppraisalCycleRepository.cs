using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IAppraisalCycleRepository : IGenericRepository<AppraisalCycle>
{
    Task<IEnumerable<AppraisalCycle>> GetActiveCyclesAsync();
    Task<AppraisalCycle?> GetByYearAndTypeAsync(string yearLabel, string type);
    Task<AppraisalCycle?> GetCurrentCycleAsync();
}
