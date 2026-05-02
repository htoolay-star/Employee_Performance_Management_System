using EPMS.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Audit
{
    public class AuditLog
    {
        private AuditLog() { }

        public AuditLog(long? userId, string entityName, string entityId, string action, string? oldValues, string? newValues, DateTimeOffset timestamp, string? ipAddress = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(entityName);
            ArgumentException.ThrowIfNullOrWhiteSpace(entityId);
            ArgumentException.ThrowIfNullOrWhiteSpace(action);

            UserId = userId;

            EntityName = entityName.Trim();
            EntityId = entityId.Trim();
            Action = action.Trim().ToUpperInvariant();

            OldValues = oldValues;
            NewValues = newValues;
            IpAddress = ipAddress?.Trim();

            Timestamp = timestamp;
        }

        public long Id { get; private set; }
        public long? UserId { get; private set; }
        public string EntityName { get; private set; } = string.Empty;
        public string EntityId { get; private set; } = string.Empty;
        public string Action { get; private set; } = string.Empty;

        public string? OldValues { get; private set; }
        public string? NewValues { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }
        public string? IpAddress { get; private set; }

        public virtual User? User { get; private set; }
    }
}
