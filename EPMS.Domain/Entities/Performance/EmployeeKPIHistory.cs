using EPMS.Domain.Contracts;

namespace EPMS.Domain.Entities.Performance
{
    public class EmployeeKPIHistory : BaseEntity
    {
        private EmployeeKPIHistory() { }

        public EmployeeKPIHistory(long employeeId, long cycleId, long kpiId, long priorityId,
                                  decimal weightage, string? targetValue, string? targetUnit,
                                  DateTimeOffset snapshotDate)
        {
            EmployeeId = employeeId;
            CycleId = cycleId;
            KPIId = kpiId;
            PriorityId = priorityId;
            Weightage = weightage;
            TargetValue = targetValue?.Trim();
            TargetUnit = targetUnit?.Trim();
            SnapshotDate = snapshotDate;
        }

        public long EmployeeId { get; private set; }
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
