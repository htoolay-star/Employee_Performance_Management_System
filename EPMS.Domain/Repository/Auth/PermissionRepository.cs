using EPMS.Domain.Data;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Domain.Repository.Auth;

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(AppDbContext context) : base(context) { }

    public async Task<bool> IsCodeUniqueAsync(string code, long? excludeId = null)
    {
        return !await _dbSet.AnyAsync(p => p.Code == code.ToUpper() && p.Id != excludeId);
    }

    public async Task<Permission?> GetByCodeAsync(string code)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();
        return await _dbSet.FirstOrDefaultAsync(p => p.Code == normalizedCode && p.IsActive && !p.IsDeleted);
    }
}
