using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class FormQuestionRepository : GenericRepository<FormQuestion>, IFormQuestionRepository
{
    public FormQuestionRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<FormQuestion>> GetByTemplateIdAsync(long templateId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(q => q.Category)
            .Where(q => q.TemplateId == templateId && !q.IsDeleted)
            .OrderBy(q => q.Sequence)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FormQuestion>> GetByCategoryIdAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(q => q.Category)
            .Where(q => q.CategoryId == categoryId && !q.IsDeleted)
            .OrderBy(q => q.Sequence)
            .ToListAsync(cancellationToken);
    }

    public async Task<FormQuestion?> GetByTemplateAndSequenceAsync(long templateId, int sequence, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(q => q.TemplateId == templateId && q.Sequence == sequence && !q.IsDeleted, cancellationToken);
    }

    public async Task<bool> ExistsAsync(long templateId, int sequence, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(q => q.TemplateId == templateId && q.Sequence == sequence && !q.IsDeleted, cancellationToken);
    }
}