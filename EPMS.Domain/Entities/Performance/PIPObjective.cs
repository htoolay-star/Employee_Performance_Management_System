using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class PIPObjective : AuditableEntity , ISoftDeletable
    {
        private PIPObjective() { }

        public PIPObjective(long pipId, string title, string successCriteria, string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(successCriteria);

            PIPId = pipId;
            Title = title.Trim();
            SuccessCriteria = successCriteria.Trim();
            Description = description?.Trim();
            Status = "In-Progress";
        }

        public long PIPId { get; private set; }

        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string SuccessCriteria { get; private set; } = string.Empty;

        public string Status { get; private set; } = string.Empty;
        public string? ManagerComment { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual PIP PIP { get; private set; } = null!;

        public void EvaluateObjective(string newStatus, string? comment)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newStatus);

            Status = newStatus.Trim();
            ManagerComment = comment?.Trim();
        }
    }
}
