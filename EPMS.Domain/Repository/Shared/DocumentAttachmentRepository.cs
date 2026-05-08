using EPMS.Domain.Data;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Shared;

public class DocumentAttachmentRepository : GenericRepository<DocumentAttachment>, IDocumentAttachmentRepository
{
    public DocumentAttachmentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<DocumentAttachment>> GetByEntityIdAsync(string entityType, long entityId)
    {
        return await _dbSet
            .Where(d => d.EntityType == entityType && d.EntityId == entityId && !d.IsDeleted)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync();
    }
}