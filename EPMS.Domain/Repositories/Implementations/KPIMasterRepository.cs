using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using Microsoft.EntityFrameworkCore;

public class KPIMasterRepository : IKPIMasterRepository
{
    private readonly AppDbContext _context;

    public KPIMasterRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<KPIMaster>> GetAllAsync()
    {
        return await _context.KPIMasters
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<KPIMaster?> GetByIdAsync(long id)
    {
        return await _context.KPIMasters
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task AddAsync(KPIMaster entity)
    {
        await _context.KPIMasters.AddAsync(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}