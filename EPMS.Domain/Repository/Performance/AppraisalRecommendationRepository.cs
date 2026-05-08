using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance;

public class AppraisalRecommendationRepository : GenericRepository<AppraisalRecommendation>, IAppraisalRecommendationRepository
{
    public AppraisalRecommendationRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<AppraisalRecommendation>> GetByAppraisalIdAsync(long appraisalId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.AppraisalId == appraisalId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<AppraisalRecommendation?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Appraisal)
            .Include(r => r.ProcessedBy)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);
    }
}