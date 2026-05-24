using EPMS.Domain.Data;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Repository.Base;
using EPMS.Shared.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Shared;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<LookUpDto>> GetLookupAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Select(x => new LookUpDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
            })
            .ToListAsync();
    }        
    public async Task<bool> ExistsByCodeAsync(string code, long? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Code == code);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, long? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Name == name);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<bool> HasSubCategoriesAsync(long categoryId)
    {
        return await _dbSet.AnyAsync(c => c.ParentId == categoryId);
    }
}
