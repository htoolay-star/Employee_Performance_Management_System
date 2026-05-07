using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Performance
{
    public class KPIMasterRepository : GenericRepository<KPIMaster>, IKPIMasterRepository
    {
        public KPIMasterRepository(AppDbContext context) : base(context) { }

        public async Task<bool> ExistsByCodeAsync(string code, long? excludeId = null)
        {
            var normalized = code.Trim().ToUpperInvariant();
            return await _dbSet.AnyAsync(k => k.Code == normalized && k.Id != excludeId && !k.IsDeleted);
        }

        public async Task<IEnumerable<KPIMaster>> GetActiveKPIsAsync()
        {
            return await _dbSet.Where(k => k.IsActive && !k.IsDeleted).ToListAsync();
        }
    }
}
