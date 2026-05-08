using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IEvaluationResponseRepository : IGenericRepository<EvaluationResponse>
{
    Task<IEnumerable<EvaluationResponse>> GetByAppraisalIdAsync(long appraisalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EvaluationResponse>> GetByTemplateIdAsync(long templateId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EvaluationResponse>> GetByQuestionIdAsync(long questionId, CancellationToken cancellationToken = default);
    Task<EvaluationResponse?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default);
}