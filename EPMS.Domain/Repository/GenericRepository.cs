using EPMS.Domain.Data;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo;
using Microsoft.EntityFrameworkCore;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    
    Task<Department?> GetDepartmentWithTeamsAsync(long id);
}

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context) { }

    public async Task<Department?> GetDepartmentWithTeamsAsync(long id)
    {
        return await _context.Departments
            .Include(d => d.Teams)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}