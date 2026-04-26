using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
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

        public DbSeeder(IUnitOfWork uow, IOptions<SeedSettings> options)
        {
            _uow = uow;
            _settings = options.Value;
        }

        public async Task SeedAsync()
        {
            await SeedSystemAdminAsync();
        }

        private async Task SeedSystemAdminAsync()
        {
            if (await _uow.Auth.Users.ExistsAsync(_settings.SAEmail)) return;

            await _uow.BeginTransactionAsync();

            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(_settings.SAPassword);

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
