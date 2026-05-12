using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;

public class PositionKPIRepository : IPositionKPIRepository
{
    private readonly AppDbContext _context;

    public PositionKPIRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PositionKPI>> GetByPositionIdAsync(long positionId)
    {
        return await _context.PositionKPIs
            .Include(x => x.KPI)
            .Include(x => x.Priority)
            .Where(x => x.PositionId == positionId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<PositionKPI?> GetByIdAsync(long id)
    {
        return await _context.PositionKPIs
            .Include(x => x.KPI)
            .Include(x => x.Priority)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(PositionKPI entity)
    {
        await _context.PositionKPIs.AddAsync(entity);
    }

    public async Task<decimal> GetTotalWeightByPositionAsync(long positionId)
    {
        return await _context.PositionKPIs
            .Where(x => x.PositionId == positionId && !x.IsDeleted)
            .SumAsync(x => x.Weightage);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}