using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;

namespace EPMS.Domain.Interface.Irepo.Auth
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
    }
}
