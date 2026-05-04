using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using System;

namespace EPMS.Domain.Entities.Performance
{
    public class PIPStatusHistory : BaseEntity
    {
        private PIPStatusHistory() { }

        public PIPStatusHistory(
            long pipId,
            string fromStatus,
            string toStatus,
            long changedById,
            string? reason = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(toStatus);

            PIPId = pipId;
            FromStatus = fromStatus?.Trim() ?? "NULL";
            ToStatus = toStatus.Trim().ToUpperInvariant();
            ChangedById = changedById;
            Reason = reason?.Trim();
            ChangedAt = DateTimeOffset.UtcNow;
        }

        public long PIPId { get; private set; }

        public string FromStatus { get; private set; } = string.Empty;
        public string ToStatus { get; private set; } = string.Empty;

        public long ChangedById { get; private set; }
        public DateTimeOffset ChangedAt { get; private set; }
        public string? Reason { get; private set; }

        public virtual PIP PIP { get; private set; } = null!;
        public virtual User ChangedBy { get; private set; } = null!;
    }
}
