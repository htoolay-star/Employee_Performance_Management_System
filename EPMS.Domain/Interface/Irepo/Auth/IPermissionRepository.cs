using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;

namespace EPMS.Domain.Interface.Irepo.Auth;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<bool> IsCodeUniqueAsync(string code, long? excludeId = null);
    Task<Permission?> GetByCodeAsync(string code);
}
