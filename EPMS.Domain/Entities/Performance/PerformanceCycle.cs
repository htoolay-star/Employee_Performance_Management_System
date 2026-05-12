using EPMS.Domain.Contracts;

namespace EPMS.Domain.Entities.Performance;

public class PerformanceCycle : AuditableEntity
{
    private PerformanceCycle() { }

    public PerformanceCycle(
        string name,
        DateOnly startDate,
        DateOnly endDate)
    {
        Name = name.Trim();
        StartDate = startDate;
        EndDate = endDate;
    }

    public string Name { get; private set; } = string.Empty;

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public bool IsClosed { get; private set; }

    public void Close()
    {
        IsClosed = true;
    }
}
