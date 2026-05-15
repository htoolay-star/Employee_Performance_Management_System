using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Entities.Performance
{
    public class DeptKPI : AuditableEntity, ISoftDeletable
    {
        private DeptKPI() { }

        public DeptKPI(long deptId, long kpiId, KPIWeightPriority priority, decimal weightage, string? targetValue, string? targetUnit)
        {
            if (!priority.IsValidWeight(weightage))
                throw new ArgumentException($"Weightage {weightage} falls outside the allowed bounds for {priority.LevelName}.");

            DeptId = deptId;
            KPIId = kpiId;
            PriorityId = priority.Id;
            Weightage = weightage;

            TargetValue = targetValue?.Trim();
            TargetUnit = targetUnit?.Trim();
        }

        public long DeptId { get; private set; }
        public long KPIId { get; private set; }
        public long PriorityId { get; private set; }

        public string? TargetValue { get; private set; }
        public string? TargetUnit { get; private set; }
        public decimal Weightage { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Department Department { get; private set; } = null!;
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
    }
}
