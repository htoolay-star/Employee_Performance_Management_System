using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class PIP : AuditableEntity , ISoftDeletable
    {
        private PIP() { }

        public PIP(long employeeId, long managerId, DateOnly startDate, DateOnly endDate, string reason, long? appraisalId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reason);
            if (startDate >= endDate) throw new ArgumentException("Start date must be before end date.");

            EmployeeId = employeeId;
            ManagerId = managerId;
            AppraisalId = appraisalId;
            StartDate = startDate;
            EndDate = endDate;

            Reason = reason.Trim();
            Status = PIPStatuses.Open;
        }

        public long EmployeeId { get; private set; }
        public long ManagerId { get; private set; }
        public long? AppraisalId { get; private set; }

        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }

        public string Reason { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;
        public string? FinalOutcomeNotes { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual EmployeeProfile Employee { get; private set; } = null!;
        public virtual EmployeeProfile Manager { get; private set; } = null!;
        public virtual Appraisal? OriginAppraisal { get; private set; }

        private readonly List<PIPObjective> _objectives = new();
        public virtual IReadOnlyCollection<PIPObjective> Objectives => _objectives.AsReadOnly();

        public void AddObjective(PIPObjective objective)
        {
            ArgumentNullException.ThrowIfNull(objective);
            _objectives.Add(objective);
        }

        public void ConcludePIP(bool isSuccessful, string? notes)
        {
            if (Status is PIPStatuses.Successful or PIPStatuses.Failed)
                throw new InvalidOperationException("PIP is already concluded.");

            Status = isSuccessful ? PIPStatuses.Successful : PIPStatuses.Failed;
            FinalOutcomeNotes = notes?.Trim();
        }

        public void ExtendPIP(DateOnly newEndDate, string reasonExtension)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reasonExtension);
            if (newEndDate <= EndDate) throw new ArgumentException("Extension date must be later than the current end date.");

            EndDate = newEndDate;
            Status = PIPStatuses.Extended;
            Reason += $"\n\n[Extended]: {reasonExtension.Trim()}";
        }
    }
}
