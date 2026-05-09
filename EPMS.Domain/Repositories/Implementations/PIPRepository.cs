using EPMS.Domain.Data;
using EPMS.Domain.Entities;
using EPMS.Domain.Entities.PerformanceImprovementPlan;
using EPMS.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PIPRepository : IPIPRepository
{
    private readonly AppDbContext _context;

    public PIPRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PerformanceImprovementPlan pip)
    {
        await _context.PIPs.AddAsync(pip);
        await _context.SaveChangesAsync();
    }

    public async Task<PerformanceImprovementPlan?> GetByIdAsync(Guid id)
    {
        return await _context.PIPs
            .Include(p => p.ProgressUpdates) // important for tracking progress
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<PerformanceImprovementPlan>> GetAllAsync()
    {
        return await _context.PIPs
            .Include(p => p.ProgressUpdates)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
