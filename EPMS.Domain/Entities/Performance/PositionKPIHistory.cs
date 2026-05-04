using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Hr;
using System;

namespace EPMS.Domain.Entities.Performance
{
    public class PositionKPIHistory : BaseEntity
    {
        private PositionKPIHistory() { }

        public PositionKPIHistory(
            long positionId,
            long kpiId,
            long priorityId,
            decimal weightage,
            string? targetValue,
            string? targetUnit,
            DateOnly effectiveDate,
            string? changeReason = null,
            long? changedById = null)
        {
            PositionId = positionId;
            KPIId = kpiId;
            PriorityId = priorityId;
            Weightage = weightage;
            TargetValue = targetValue?.Trim();
            TargetUnit = targetUnit?.Trim();
            EffectiveDate = effectiveDate;
            ChangeReason = changeReason?.Trim();
            ChangedById = changedById;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public long PositionId { get; private set; }
        public long KPIId { get; private set; }
        public long PriorityId { get; private set; }

        public decimal Weightage { get; private set; }
        public string? TargetValue { get; private set; }
        public string? TargetUnit { get; private set; }

        public DateOnly EffectiveDate { get; private set; }
        public DateOnly? EndDate { get; private set; }

        public string? ChangeReason { get; private set; }
        public long? ChangedById { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public virtual Position Position { get; private set; } = null!;
        public virtual KPIMaster KPI { get; private set; } = null!;
        public virtual KPIWeightPriority Priority { get; private set; } = null!;
        public virtual User? ChangedBy { get; private set; }

        public void EndAssignment(DateOnly endDate)
        {
            if (endDate < EffectiveDate)
                throw new ArgumentException("End date cannot be before effective date.");

            EndDate = endDate;
        }
    }
}
