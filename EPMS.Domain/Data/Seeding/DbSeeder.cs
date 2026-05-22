using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.Enums;
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
            await SeedRolesAsync();
            await SeedSystemAdminAsync();
            await SeedPermissionsAsync();
        }

        private async Task SeedSystemSettingsAsync()
        {
            var setting = await _uow.App.SystemSettings.GetByKeyAsync("DefaultUserPassword");

            if (setting == null)
            {
                var encryptedPw = _cryptoService.Encrypt(_settings.DefaultUserPassword);

                var defaultPwSetting = new SystemSetting(
                    "DefaultUserPassword",
                    encryptedPw,
                    "Initial default password assigned to newly created users (AES Encrypted)."
                );

                _uow.App.SystemSettings.Add(defaultPwSetting);
                await _uow.CompleteAsync();
            }
        }

        private async Task SeedRolesAsync()
        {
            var existingRoles = await _uow.Auth.Roles.GetAllAsync();
            if (existingRoles.Any()) return;

            var roles = new List<Role>
            {
                new Role(1, "SystemAdmin", "Technical support & Emergency troubleshooting only"),
                new Role(2, "Admin", "Power user for HR & Operations (No Role assignment)"),
                new Role(3, "User", "Standard employee access")
            };

            foreach (var role in roles)
            {
                _uow.Auth.Roles.Add(role);
            }

            await _uow.CompleteAsync();
        }

        private async Task SeedSystemAdminAsync()
        {
            if (await _uow.Auth.Users.ExistsAsync(_settings.SAEmail)) return;

            await _uow.BeginTransactionAsync();

            try
            {
                var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(_settings.SAPassword, 12);

                var adminUser = User.CreateSystemAdmin(_settings.SAEmail, passwordHash);

                _uow.Auth.Users.Add(adminUser);
                await _uow.CompleteAsync();

                var adminProfile = new EmployeeProfile(
                    userId: adminUser.Id,
                    staffNo: "SYS-001",
                    staffName: "System Administrator",
                    email: _settings.SAEmail
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

        private async Task SeedPermissionsAsync()
        {
            var existingPermissions = await _uow.Auth.Permissions.GetAllAsync();
            if (existingPermissions.Any()) return;

            var permissions = new List<Permission>
            {
                // Positions
                new("POSITIONS.VIEW", "View Positions"),
                new("POSITIONS.CREATE", "Create Positions"),
                new("POSITIONS.EDIT", "Edit Positions"),
                new("POSITIONS.DELETE", "Delete Positions"),
                // Departments
                new("DEPARTMENTS.VIEW", "View Departments"),
                new("DEPARTMENTS.CREATE", "Create Departments"),
                new("DEPARTMENTS.EDIT", "Edit Departments"),
                new("DEPARTMENTS.DELETE", "Delete Departments"),
                // Teams
                new("TEAMS.VIEW", "View Teams"),
                new("TEAMS.CREATE", "Create Teams"),
                new("TEAMS.EDIT", "Edit Teams"),
                new("TEAMS.DELETE", "Delete Teams"),
                // Levels
                new("LEVELS.VIEW", "View Levels"),
                new("LEVELS.CREATE", "Create Levels"),
                new("LEVELS.EDIT", "Edit Levels"),
                new("LEVELS.DELETE", "Delete Levels"),
                // Entity KPIs
                new("ENTITYKPI.VIEW", "View Entity KPIs"),
                new("ENTITYKPI.CREATE", "Create Entity KPIs"),
                new("ENTITYKPI.EDIT", "Edit Entity KPIs"),
                new("ENTITYKPI.DELETE", "Delete Entity KPIs"),
                // Employee KPIs
                new("EMPLOYEEKPI.VIEW", "View Employee KPIs"),
                new("EMPLOYEEKPI.CREATE", "Create Employee KPIs"),
                new("EMPLOYEEKPI.EDIT", "Edit Employee KPIs"),
                new("EMPLOYEEKPI.DELETE", "Delete Employee KPIs"),
                // Position Form Templates
                new("POSITIONFORMTPL.VIEW", "View Position Form Templates"),
                new("POSITIONFORMTPL.CREATE", "Create Position Form Templates"),
                new("POSITIONFORMTPL.EDIT", "Edit Position Form Templates"),
                new("POSITIONFORMTPL.DELETE", "Delete Position Form Templates"),
                // Form Templates
                new("FORMTEMPLATE.VIEW", "View Form Templates"),
                new("FORMTEMPLATE.CREATE", "Create Form Templates"),
                new("FORMTEMPLATE.EDIT", "Edit Form Templates"),
                new("FORMTEMPLATE.DELETE", "Delete Form Templates"),
            };

            foreach (var p in permissions)
            {
                _uow.Auth.Permissions.Add(p);
            }

            await _uow.CompleteAsync();
        }
    }
}
