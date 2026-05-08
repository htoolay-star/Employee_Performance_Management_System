using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class EvaluationResponseRepository : GenericRepository<EvaluationResponse>, IEvaluationResponseRepository
{
    public EvaluationResponseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<EvaluationResponse>> GetByAppraisalIdAsync(long appraisalId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.AppraisalId == appraisalId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EvaluationResponse>> GetByTemplateIdAsync(long templateId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.TemplateId == templateId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EvaluationResponse>> GetByQuestionIdAsync(long questionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.QuestionId == questionId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<EvaluationResponse?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Appraisal)
            .Include(r => r.Template)
            .Include(r => r.Question)
            .Include(r => r.Evaluator)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);
    }
}