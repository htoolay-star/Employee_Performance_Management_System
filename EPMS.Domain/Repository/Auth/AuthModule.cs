using EPMS.Domain.Data;
using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Interface.Irepo.Auth;
using EPMS.Domain.Repository.App;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Auth
{
    public class AuthModule(IServiceProvider serviceProvider) : IAuthModule
    {
    private IUserRepository? _users;
    private IUserRefreshTokenRepository? _usersRefreshToken;
    private IRoleRepository? _roles;
    private IPermissionRepository? _permissions;
    private IPositionPermissionRepository? _positionPermissions;
    private IPositionRoleRepository? _positionRoles;
    private IPasswordResetOtpRepository? _passwordResetOtps;
    private IPasswordResetRequestRepository? _passwordResetRequests;

        public IUserRepository Users =>
        _users ??= serviceProvider.GetRequiredService<IUserRepository>();

        public IUserRefreshTokenRepository UsersRefreshToken =>
        _usersRefreshToken ??= serviceProvider.GetRequiredService<IUserRefreshTokenRepository>();

        public IRoleRepository Roles =>
        _roles ??= serviceProvider.GetRequiredService<IRoleRepository>();

        public IPermissionRepository Permissions =>
        _permissions ??= serviceProvider.GetRequiredService<IPermissionRepository>();

        public IPositionPermissionRepository PositionPermissions =>
        _positionPermissions ??= serviceProvider.GetRequiredService<IPositionPermissionRepository>();

        public IPositionRoleRepository PositionRoles =>
        _positionRoles ??= serviceProvider.GetRequiredService<IPositionRoleRepository>();

        public IPasswordResetOtpRepository PasswordResetOtps =>
        _passwordResetOtps ??= serviceProvider.GetRequiredService<IPasswordResetOtpRepository>();

        public IPasswordResetRequestRepository PasswordResetRequests =>
        _passwordResetRequests ??= serviceProvider.GetRequiredService<IPasswordResetRequestRepository>();
    }
}
