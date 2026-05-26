using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class PositionFormTemplateRepository : GenericRepository<PositionFormTemplate>, IPositionFormTemplateRepository
{
    public PositionFormTemplateRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PositionFormTemplate>> GetByPositionIdAsync(long positionId)
    {
        return await _dbSet
            .Where(p => p.PositionId == positionId && !p.IsDeleted)
            .Include(p => p.FormTemplate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PositionFormTemplate>> GetByPositionIdWithQuestionsAsync(long positionId)
    {
        return await _dbSet
            .Where(p => p.PositionId == positionId && !p.IsDeleted)
            .Include(p => p.FormTemplate)
                .ThenInclude(t => t.Questions)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(long positionId, long formTemplateId)
    {
        return await _dbSet.AnyAsync(p =>
            p.PositionId == positionId &&
            p.FormTemplateId == formTemplateId &&
            !p.IsDeleted);
    }
}