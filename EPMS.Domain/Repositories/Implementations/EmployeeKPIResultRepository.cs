using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;

public class EmployeeKPIResultRepository
    : IEmployeeKPIResultRepository
{
    private readonly AppDbContext _context;

    public EmployeeKPIResultRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EmployeeKPIResult entity)
    {
        await _context.EmployeeKPIResults.AddAsync(entity);
    }

    public async Task<List<EmployeeKPIResult>> GetByEmployeeAsync(
        long employeeId,
        long cycleId)
    {
        return await _context.EmployeeKPIResults
            .Include(x => x.PositionKPI)
                .ThenInclude(x => x.KPI)
            .Where(x =>
                x.EmployeeId == employeeId &&
                x.PerformanceCycleId == cycleId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}