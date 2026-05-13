using EPMS.Domain.Data;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Shared;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<(long Id, string Code)>> GetLookupAsync()
    {
        return await _dbSet
            .Select(x => new ValueTuple<long, string>(x.Id, x.Name))
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, string? module = null)
    {
        var normalizedName = name.Trim().ToLowerInvariant();
        var query = _dbSet.Where(t => t.Name == normalizedName);
        
        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(t => t.Module == module);
        
        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, string? module, long excludeId)
    {
        var normalizedName = name.Trim().ToLowerInvariant();
        var query = _dbSet.Where(t => t.Name == normalizedName && t.Id != excludeId);
        
        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(t => t.Module == module);
        
        return await query.AnyAsync();
    }
}
