using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IAppraisalRecommendationRepository : IGenericRepository<AppraisalRecommendation>
{
    Task<IEnumerable<AppraisalRecommendation>> GetByAppraisalIdAsync(long appraisalId, CancellationToken cancellationToken = default);
    Task<AppraisalRecommendation?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default);
}