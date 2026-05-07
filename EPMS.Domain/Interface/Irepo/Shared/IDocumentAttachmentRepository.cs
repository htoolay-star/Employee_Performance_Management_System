using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;

namespace EPMS.Domain.Interface.Irepo.Shared;

public interface IDocumentAttachmentRepository : IGenericRepository<DocumentAttachment>
{
    Task<IEnumerable<DocumentAttachment>> GetByEntityIdAsync(string entityType, long entityId);
}