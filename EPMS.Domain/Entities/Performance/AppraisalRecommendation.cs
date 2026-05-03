using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class AppraisalRecommendation : AuditableEntity , ISoftDeletable
    {
        private AppraisalRecommendation() { }

        public AppraisalRecommendation(long appraisalId, string type, string reason, string? proposedValue = null, string priority = "Normal")
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(type);
            ArgumentException.ThrowIfNullOrWhiteSpace(reason);
            ArgumentException.ThrowIfNullOrWhiteSpace(priority);

            AppraisalId = appraisalId;

            RecommendationType = type.Trim().ToUpperInvariant();
            Reason = reason.Trim();
            ProposedValue = proposedValue?.Trim();
            Priority = priority.Trim().ToUpperInvariant();

            Status = "Pending";
        }

        public long AppraisalId { get; private set; }

        public string RecommendationType { get; private set; } = string.Empty;
        public string? ProposedValue { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public string Priority { get; private set; } = string.Empty;

        public string Status { get; private set; } = string.Empty;
        public string? HRComments { get; private set; }

        public long? ProcessedById { get; private set; }
        public DateTimeOffset? ActionDate { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Appraisal Appraisal { get; private set; } = null!;
        public virtual EmployeeProfile? ProcessedBy { get; private set; }

        public void Approve(long hrAdminId, string? comments)
        {
            if (Status != "Pending") throw new InvalidOperationException("Only pending recommendations can be approved.");

            Status = "Approved";
            HRComments = comments?.Trim();
            ProcessedById = hrAdminId;
            ActionDate = DateTimeOffset.UtcNow;
        }

        public void Reject(long hrAdminId, string reason)
        {
            if (Status != "Pending") throw new InvalidOperationException("Only pending recommendations can be rejected.");

            ArgumentException.ThrowIfNullOrWhiteSpace(reason);

            Status = "Rejected";
            HRComments = reason.Trim();
            ProcessedById = hrAdminId;
            ActionDate = DateTimeOffset.UtcNow;
        }
    }
}
