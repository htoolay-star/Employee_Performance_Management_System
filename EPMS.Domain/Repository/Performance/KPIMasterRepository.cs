using EPMS.Domain.Data;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.Irepo.Performance;
using EPMS.Domain.Repository.Base;
using EPMS.Shared.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Performance
{
    public class KPIMasterRepository : GenericRepository<KPIMaster>, IKPIMasterRepository
    {
        public KPIMasterRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<KPIMaster>> GetActiveAsync()
        {
            return await _dbSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .Include(x => x.Category)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<KPIMaster?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
        }

        public async Task<bool> CodeExistsAsync(string code, long? excludeId = null)
        {
            return await _dbSet
                .AnyAsync(x => x.Code == code && !x.IsDeleted && (excludeId == null || x.Id != excludeId));
        }

        public async Task<IEnumerable<LookUpDto>> GetLookupDtoAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(x => new LookUpDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                })
                .ToListAsync();
        }
    }
}