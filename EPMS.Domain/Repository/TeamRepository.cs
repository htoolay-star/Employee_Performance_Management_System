// Interface
using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo;
using Microsoft.EntityFrameworkCore;

public interface ITeamRepository : IGenericRepository<Team>
{
    Task<IEnumerable<Team>> GetTeamsByDepartmentAsync(long departmentId);
}

// Implementation
public class TeamRepository : GenericRepository<Team>, ITeamRepository
{
    public TeamRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Team>> GetTeamsByDepartmentAsync(long departmentId)
    {
        return await _context.Teams
            .Where(t => t.DepartmentId == departmentId && t.IsActive)
            .ToListAsync();
    }
}