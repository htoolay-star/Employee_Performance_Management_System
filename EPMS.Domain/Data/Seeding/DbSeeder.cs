using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.Enums.EPMS.Shared.Enums;
using EPMS.Shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Data.Seeding
{
    public class DbSeeder : IDbSeeder
    {
        private readonly IUnitOfWork _uow;
        private readonly SeedSettings _settings;
        private readonly ICryptoService _cryptoService;

        public DbSeeder(IUnitOfWork uow, IOptions<SeedSettings> options, ICryptoService cryptoService)
        {
            _uow = uow;
            _settings = options.Value;
            _cryptoService = cryptoService;
        }

        public async Task SeedAsync()
        {
            await SeedSystemSettingsAsync();
            await SeedSystemAdminAsync();
        }

        private async Task SeedSystemSettingsAsync()
        {
            var setting = await _uow.Auth.SystemSettings.GetByKeyAsync("DefaultUserPassword");

            if (setting == null)
            {
                var encryptedPw = _cryptoService.Encrypt(_settings.DefaultUserPassword);

                var defaultPwSetting = new SystemSetting(
                    "DefaultUserPassword",
                    encryptedPw,
                    "Initial default password assigned to newly created users (AES Encrypted)."
                );

                _uow.Auth.SystemSettings.Add(defaultPwSetting);
                await _uow.CompleteAsync();
            }
        }

        private async Task SeedSystemAdminAsync()
        {
            if (await _uow.Auth.Users.ExistsAsync(_settings.SAEmail)) return;

            await _uow.BeginTransactionAsync();

            try
            {
                var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(_settings.SAPassword, 12);

                var adminUser = new User(_settings.SAEmail, passwordHash, UserRole.SystemAdmin);

                _uow.Auth.Users.Add(adminUser);
                await _uow.CompleteAsync();

                var adminProfile = new EmployeeProfile(
                    userId: adminUser.Id,
                    staffNo: "SYS-001",
                    firstName: "System",
                    lastName: "Administrator"
                    );

                _uow.Info.EmployeeProfiles.Add(adminProfile);

                await _uow.CompleteAsync();
                await _uow.CommitAsync();
            }
            catch (Exception)
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
