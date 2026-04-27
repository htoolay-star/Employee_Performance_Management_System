using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.Irepo.App
{
    public interface ISystemSettingsRepository : IGenericRepository<SystemSetting>
    {
        Task<SystemSetting?> GetByKeyAsync(string key);
    }
}
