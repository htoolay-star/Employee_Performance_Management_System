using EPMS.Domain.Data;
using EPMS.Domain.Entities.Shared;
using EPMS.Domain.Interface.Irepo.Shared;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Shared;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    public async Task<bool> ExistsByCodeAsync(string code, string module, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Code == code && c.Module == module);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, string module, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Name == name && c.Module == module);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<bool> HasSubCategoriesAsync(int categoryId)
    {
        return await _dbSet.AnyAsync(c => c.ParentId == categoryId);
    }
}
