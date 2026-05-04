using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Interface.IService.App;
using EPMS.Domain.Repository.Base;
using EPMS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Services.App
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICryptoService _cryptoService;

        public SystemSettingsService(IUnitOfWork uow, ICryptoService cryptoService)
        {
            _uow = uow;
            _cryptoService = cryptoService;
        }

        public async Task<string?> GetSettingValueAsync(string key)
        {
            var setting = await _uow.App.SystemSettings.GetByKeyAsync(key);
            return setting?.Value;
        }

        public async Task<string> GetDefaultPasswordAsync()
        {
            const string key = SettingKeys.DefaultUserPassword;

            var setting = await _uow.App.SystemSettings.GetByKeyAsync(key);

            if (setting == null)
            {
                throw new Exception($"System Setting '{key}' is missing in the database.");
            }

            var plainPassword = _cryptoService.Decrypt(setting.Value);

            if (string.IsNullOrEmpty(plainPassword))
            {
                throw new Exception("Failed to decrypt the default password. Check your Encryption Key/IV.");
            }

            return plainPassword;
        }

        public async Task UpdateSettingAsync(string key, string newValue)
        {
            var setting = await _uow.App.SystemSettings.GetByKeyAsync(key, trackChanges: true);

            if (setting == null) throw new Exception("Setting not found.");

            if (key == SettingKeys.DefaultUserPassword)
            {
                newValue = _cryptoService.Encrypt(newValue);
            }

            setting.UpdateValue(newValue);
            await _uow.CompleteAsync();
        }

        public async Task UpdateDefaultPasswordAsync(string newPlainPassword)
        {
            var encryptedValue = _cryptoService.Encrypt(newPlainPassword);

            var setting = await _uow.App.SystemSettings.GetByKeyAsync(SettingKeys.DefaultUserPassword, trackChanges: true);

            if (setting == null)
            {
                setting = new SystemSetting(SettingKeys.DefaultUserPassword, encryptedValue, "Default password for new users.");
                _uow.App.SystemSettings.Add(setting);
            }
            else
            {
                setting.UpdateValue(encryptedValue);
            }

            await _uow.CompleteAsync();
        }
    }
}
