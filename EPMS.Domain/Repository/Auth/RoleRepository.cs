using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Hr;
using EPMS.Domain.Interface.Irepo;
using EPMS.Domain.Interface.Irepo.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Auth
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context) { }

        public async Task<Role?> GetByNameAsync(string name) =>
            await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }
}
