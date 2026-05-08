using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class PIPObjectiveRepository : GenericRepository<PIPObjective>, IPIPObjectiveRepository
{
    public PIPObjectiveRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PIPObjective>> GetByPIPIdAsync(long pipId)
    {
        return await _dbSet
            .Where(o => o.PIPId == pipId && !o.IsDeleted)
            .OrderBy(o => o.Title)
            .ToListAsync();
    }
}