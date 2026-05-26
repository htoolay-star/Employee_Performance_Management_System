using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Auth
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context) { }

        public async Task<Role?> GetByNameAsync(string name) =>
            await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }
}
