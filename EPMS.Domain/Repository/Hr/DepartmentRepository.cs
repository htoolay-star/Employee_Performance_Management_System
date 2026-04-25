using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Hr;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context) { }

    public async Task<Department?> GetDepartmentWithTeamsAsync(long id)
    {
        return await _context.Departments
            .Include(d => d.Teams)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _dbSet.AnyAsync(d => d.Code == code);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _dbSet.AnyAsync(d => d.Name == name);
    }
}
