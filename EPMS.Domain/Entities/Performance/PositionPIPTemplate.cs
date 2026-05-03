using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class PositionPIPTemplate : AuditableEntity , ISoftDeletable
    {
        private PositionPIPTemplate() { }

        public PositionPIPTemplate(long positionId, string title, string successCriteria, string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(successCriteria);

            PositionId = positionId;

            Title = title.Trim();
            SuccessCriteria = successCriteria.Trim();
            Description = description?.Trim();

            IsActive = true;
        }

        public long PositionId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string SuccessCriteria { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Position Position { get; private set; } = null!;

        public void UpdateDetails(string title, string successCriteria, string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(successCriteria);

            Title = title.Trim();
            SuccessCriteria = successCriteria.Trim();
            Description = description?.Trim();
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
