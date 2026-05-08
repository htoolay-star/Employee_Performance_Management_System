using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class PositionPIPTemplateRepository : GenericRepository<PositionPIPTemplate>, IPositionPIPTemplateRepository
{
    public PositionPIPTemplateRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PositionPIPTemplate>> GetByPositionIdAsync(long positionId)
    {
        return await _dbSet
            .Where(p => p.PositionId == positionId && !p.IsDeleted)
            .OrderBy(p => p.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<PositionPIPTemplate>> GetActiveByPositionIdAsync(long positionId)
    {
        return await _dbSet
            .Where(p => p.PositionId == positionId && p.IsActive && !p.IsDeleted)
            .OrderBy(p => p.Title)
            .ToListAsync();
    }
}