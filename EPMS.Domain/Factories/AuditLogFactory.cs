using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPMS.Domain.Factories
{
    public sealed class AuditLogFactory(TimeProvider timeProvider) : IAuditLogFactory
    {
        public List<AuditLog> CreateAuditLogs(IEnumerable<EntityEntry<IAuditableEntity>> entries, long? userId)
        {
            List<AuditLog> auditLogs = [];
            var timestamp = timeProvider.GetUtcNow();

            foreach (var entry in entries)
            {
                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();

                var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
                var entityId = primaryKey?.CurrentValue?.ToString() ?? "Unknown";

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary) continue;

                    string propertyName = property.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            newValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            oldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                oldValues[propertyName] = property.OriginalValue;
                                newValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }

                auditLogs.Add(new AuditLog(
                    userId: userId,
                    entityName: entry.Entity.GetType().Name,
                    entityId: entityId,
                    action: entry.State.ToString().ToUpperInvariant(),
                    oldValues: oldValues.Count > 0 ? JsonSerializer.Serialize(oldValues) : null,
                    newValues: newValues.Count > 0 ? JsonSerializer.Serialize(newValues) : null,
                    timestamp: timestamp));
            }

            return auditLogs;
        }
    }
}
