using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.App;

namespace EPMS.Domain.Interface.Irepo.App
{
    public interface ISystemSettingsRepository : IGenericRepository<SystemSetting>
    {
        Task<SystemSetting?> GetByKeyAsync(string key, bool trackChanges = false);
    }
}
