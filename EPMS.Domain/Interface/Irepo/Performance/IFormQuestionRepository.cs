using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance;

public interface IFormQuestionRepository : IGenericRepository<FormQuestion>
{
    Task<IEnumerable<FormQuestion>> GetByTemplateIdAsync(long templateId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FormQuestion>> GetByCategoryIdAsync(long categoryId, CancellationToken cancellationToken = default);
    Task<FormQuestion?> GetByTemplateAndSequenceAsync(long templateId, int sequence, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long templateId, int sequence, CancellationToken cancellationToken = default);
}