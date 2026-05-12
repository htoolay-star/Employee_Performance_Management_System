using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Entities.Performance;

public class EmployeeKPIResult : AuditableEntity
{
    private EmployeeKPIResult() { }

    public EmployeeKPIResult(
        long employeeId,
        long positionKpiId,
        long performanceCycleId,
        decimal targetValue,
        decimal actualValue,
        bool isNegativeKpi)
    {
        EmployeeId = employeeId;
        PositionKPIId = positionKpiId;
        PerformanceCycleId = performanceCycleId;

        TargetValue = targetValue;
        ActualValue = actualValue;
        IsNegativeKpi = isNegativeKpi;

        Calculate();
    }

    public long EmployeeId { get; private set; }

    public long PositionKPIId { get; private set; }

    public long PerformanceCycleId { get; private set; }

    public decimal TargetValue { get; private set; }

    public decimal ActualValue { get; private set; }

    public decimal ScorePercentage { get; private set; }

    public decimal WeightedScore { get; private set; }

    public bool IsNegativeKpi { get; private set; }

    public virtual EmployeeKPIResult Employee { get; private set; } = null!; //

    public virtual PositionKPI PositionKPI { get; private set; } = null!;

    public virtual PerformanceCycle PerformanceCycle { get; private set; } = null!;

    private void Calculate()
    {
        decimal score;

        if (IsNegativeKpi)
        {
            score = (TargetValue / ActualValue) * 100;
        }
        else
        {
            score = (ActualValue / TargetValue) * 100;
        }

        // Score Capping
        if (score > 100)
            score = 100;

        ScorePercentage = Math.Round(score, 2);

        WeightedScore = Math.Round(
            (ScorePercentage / 100) * PositionKPI.Weightage,
            2);
    }
}