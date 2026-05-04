using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Factories
{
    public interface IAuditLogFactory
    {
        List<AuditLog> CreateAuditLogs(IEnumerable<EntityEntry<IAuditableEntity>> entries, long? userId);
    }
}
