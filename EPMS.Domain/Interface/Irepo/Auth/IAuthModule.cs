namespace EPMS.Domain.Interface.Irepo.Auth
{
    public interface IAuthModule
    {
        IUserRepository Users { get; }
        IUserRefreshTokenRepository UsersRefreshToken { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IPositionPermissionRepository PositionPermissions { get; }
        IPositionRoleRepository PositionRoles { get; }
        IPasswordResetOtpRepository PasswordResetOtps { get; }
        IPasswordResetRequestRepository PasswordResetRequests { get; }
    }
}
