using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Auth
{
    public class AuthModule : IAuthModule
    {
        private readonly AppDbContext _context;

        private ISystemSettingsRepository? _systemSettings;
        private IUserRepository? _users;
        private IUserRefreshTokenRepository? _usersRefreshToken;
        private IRoleRepository? _roles;
        private IPermissionRepository? _permissions;

        public AuthModule(AppDbContext context) => _context = context;

        public ISystemSettingsRepository SystemSettings => _systemSettings ??= new SystemSettingsRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IUserRefreshTokenRepository UsersRefreshToken => _usersRefreshToken ??= new UserRefreshTokenRepository(_context);
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
        public IPermissionRepository Permissions => _permissions ??= new PermissionRepository(_context);
    }
}
