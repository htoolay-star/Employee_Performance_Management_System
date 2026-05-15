using EPMS.Domain.Contracts;

namespace EPMS.Domain.Entities.Performance
{
    public class EntityKPIHistory : BaseEntity
    {
        private EntityKPIHistory() { }

        public EntityKPIHistory(string entityType, long entityId, long cycleId,
                                long kpiId, long priorityId, decimal weightage,
                                string? targetValue, string? targetUnit,
                                DateTimeOffset snapshotDate)
        {
            EntityType = entityType;
            EntityId = entityId;
            CycleId = cycleId;
            KPIId = kpiId;
            PriorityId = priorityId;
            Weightage = weightage;
            TargetValue = targetValue?.Trim();
            TargetUnit = targetUnit?.Trim();
            SnapshotDate = snapshotDate;
        }

        public string EntityType { get; private set; } = string.Empty;
        public long EntityId { get; private set; }
        public long CycleId { get; private set; }
        public long KPIId { get; private set; }
        public long PriorityId { get; private set; }
        public decimal Weightage { get; private set; }
        public string? TargetValue { get; private set; }
        public string? TargetUnit { get; private set; }
        public DateTimeOffset SnapshotDate { get; private set; }

        public virtual AppraisalCycle Cycle { get; private set; } = null!;
        public virtual KPIMaster KPI { get; private set; } = null!;
        public virtual KPIWeightPriority Priority { get; private set; } = null!;
    }
}
