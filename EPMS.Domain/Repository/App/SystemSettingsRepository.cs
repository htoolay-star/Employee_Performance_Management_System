using EPMS.Domain.Data;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.App
{
    public class SystemSettingsRepository : GenericRepository<SystemSetting>, ISystemSettingsRepository
    {
        public SystemSettingsRepository(AppDbContext context) : base(context) { }

        public async Task<SystemSetting?> GetByKeyAsync(string key, bool trackChanges = false)
        {
            var query = _dbSet.AsQueryable();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(s => s.Key == key);
        }
    }
}
