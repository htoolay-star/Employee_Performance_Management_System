using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;

namespace EPMS.Domain.Interface.Irepo.Performance
{
    public interface IKPIMasterRepository : IGenericRepository<KPIMaster>
    {
        Task<IEnumerable<KPIMaster>> GetActiveAsync();
        Task<KPIMaster?> GetByCodeAsync(string code);
        Task<bool> CodeExistsAsync(string code, long? excludeId = null);
    }
}