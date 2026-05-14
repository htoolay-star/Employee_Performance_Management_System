using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class AppraisalCycleRepository : GenericRepository<AppraisalCycle>, IAppraisalCycleRepository
{
    public AppraisalCycleRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<AppraisalCycle>> GetActiveCyclesAsync()
    {
        return await _dbSet
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderByDescending(x => x.EvaluationStartDate)
            .ToListAsync();
    }

    public async Task<AppraisalCycle?> GetByYearAndTypeAsync(string yearLabel, string type)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.YearLabel == yearLabel
                                   && x.AppraisalType == type
                                   && !x.IsDeleted);
    }

    public async Task<AppraisalCycle?> GetCurrentCycleAsync()
    {
        return await _dbSet
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderByDescending(x => x.EvaluationStartDate)
            .FirstOrDefaultAsync();
    }
}
