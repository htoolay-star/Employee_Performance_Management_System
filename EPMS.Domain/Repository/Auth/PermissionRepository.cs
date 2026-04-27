using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Auth
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context) { }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            return !await _dbSet.AnyAsync(p => p.Code == code.ToUpper() && p.Id != excludeId);
        }
    }
}
