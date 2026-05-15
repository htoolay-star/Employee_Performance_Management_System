using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Entities.Performance
{
    public class EntityKPI : AuditableEntity, ISoftDeletable
    {
        private EntityKPI() { }

        public EntityKPI(string entityType, long entityId, long kpiId, KPIWeightPriority priority, decimal weightage, string? targetValue, string? targetUnit)
        {
            if (!priority.IsValidWeight(weightage))
                throw new ArgumentException($"Weightage {weightage} falls outside the allowed bounds for {priority.LevelName}.");

            EntityType = entityType;
            EntityId = entityId;
            KPIId = kpiId;
            PriorityId = priority.Id;
            Weightage = weightage;

            TargetValue = targetValue?.Trim();
            TargetUnit = targetUnit?.Trim();
        }

        public string EntityType { get; private set; } = string.Empty;
        public long EntityId { get; private set; }
        public long KPIId { get; private set; }
        public long PriorityId { get; private set; }

        public string? TargetValue { get; private set; }
        public string? TargetUnit { get; private set; }
        public decimal Weightage { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual KPIMaster KPI { get; private set; } = null!;
        public virtual KPIWeightPriority Priority { get; private set; } = null!;

        public void Update(KPIWeightPriority priority, decimal weightage, string? targetValue, string? targetUnit)
        {
            if (!priority.IsValidWeight(weightage))
                throw new ArgumentException($"Weightage {weightage} falls outside the allowed bounds for {priority.LevelName}.");

            PriorityId = priority.Id;
            Weightage = weightage;
            TargetValue = targetValue?.Trim();
            TargetUnit = targetUnit?.Trim();
        }

        public void Reassign(string entityType, long entityId)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}
