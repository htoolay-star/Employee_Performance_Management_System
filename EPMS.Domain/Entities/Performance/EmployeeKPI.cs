using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Entities.Performance;

public class EmployeeKPI : AuditableEntity, ISoftDeletable
{
    private EmployeeKPI() { }

    public EmployeeKPI(KPIWeightPriority priority, long employeeId, long kpiId, long cycleId,
                       long priorityId, decimal weightage,
                       decimal? targetValue = null, string? targetUnit = null)
    {
        if (!priority.IsValidWeight(weightage))
            throw new ArgumentException($"Weightage {weightage} falls outside the allowed bounds for {priority.LevelName}.");

        EmployeeId = employeeId;
        KPIId = kpiId;
        CycleId = cycleId;
        PriorityId = priorityId;
        Weightage = weightage;
        TargetValue = targetValue;
        TargetUnit = targetUnit?.Trim();
    }

    public long EmployeeId { get; private set; }
    public long KPIId { get; private set; }
    public long CycleId { get; private set; }
    public long PriorityId { get; private set; }

    public decimal Weightage { get; private set; }
    public decimal? TargetValue { get; private set; }
    public string? TargetUnit { get; private set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }
    public byte[] Version { get; private set; } = Array.Empty<byte>();

    public virtual EmployeeProfile Employee { get; private set; } = null!;
    public virtual KPIMaster KPI { get; private set; } = null!;
    public virtual AppraisalCycle Cycle { get; private set; } = null!;
    public virtual KPIWeightPriority Priority { get; private set; } = null!;

    public void Update(KPIWeightPriority priority, decimal weightage, decimal? targetValue, string? targetUnit)
    {
        if (!priority.IsValidWeight(weightage))
            throw new ArgumentException($"Weightage {weightage} falls outside the allowed bounds for {priority.LevelName}.");

        Weightage = weightage;
        TargetValue = targetValue;
        TargetUnit = targetUnit?.Trim();
    }
}
