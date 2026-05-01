using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService.App
{
    public interface ISystemSettingsService
    {
        Task<string?> GetSettingValueAsync(string key);

        Task<string> GetDefaultPasswordAsync();

        Task UpdateSettingAsync(string key, string newValue);
    }
}
