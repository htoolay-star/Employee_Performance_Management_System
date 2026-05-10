using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Hr;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context) { }

    public async Task<Department?> GetDepartmentWithTeamsAsync(long teamId)
    {
        return await _context.Departments
            .Include(d => d.Teams)
            .FirstOrDefaultAsync(d => d.Id == teamId);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _dbSet.AnyAsync(d => d.Code == code);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _dbSet.AnyAsync(d => d.Name == name);
    }

    public async Task<bool> ExistsByIdAsync(long id)
    {
        return await _dbSet.AnyAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<(long Id, string Name, bool IsActive)>> GetLookupAsync()
    {
        return await _dbSet
            .Select(x => new ValueTuple<long, string, bool>(x.Id, x.Name, x.IsActive))
            .ToListAsync();
    }
}
