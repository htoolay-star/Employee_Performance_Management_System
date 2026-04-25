using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo.Hr;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Hr;

public class TeamRepository : GenericRepository<Team>, ITeamRepository
{
    public TeamRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Team>> GetTeamsByDepartmentAsync(long departmentId)
    {
        return await _dbSet
            .Where(t => t.DepartmentId == departmentId && t.IsActive)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameInDepartmentAsync(string name, long departmentId)
    {
        return await _dbSet.AnyAsync(t => t.DepartmentId == departmentId && t.Name == name);
    }
}
