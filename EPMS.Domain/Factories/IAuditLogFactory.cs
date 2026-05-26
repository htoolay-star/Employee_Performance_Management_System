using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EPMS.Domain.Factories
{
    public interface IAuditLogFactory
    {
        List<AuditLog> CreateAuditLogs(IEnumerable<EntityEntry<IAuditableEntity>> entries, long? userId);
    }
}
