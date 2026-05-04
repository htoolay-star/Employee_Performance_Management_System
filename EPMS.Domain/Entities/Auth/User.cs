using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using EPMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Auth
{
    public class User : AuditableEntity , ISoftDeletable
    {
        private User() { }

        public User(string email, string passwordHash, UserRole role = UserRole.User)
        {

            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

            Email = email;
            PasswordHash = passwordHash;
            RoleId = (long)role;
            SecurityStamp = Guid.NewGuid().ToString();
            IsFirstLogin = true;
            IsActive = true;
            FailedLoginAttempts = 0;
        }

        public string Email { get; private set; } = string.Empty;
        public string NormalizedEmail { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string SecurityStamp { get; private set; } = string.Empty;

        public bool IsFirstLogin { get; private set; }
        public bool IsActive { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTimeOffset? LockoutEndDate { get; private set; }
        public DateTimeOffset? LastLoginDate { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public long RoleId { get; private set; }
        public virtual Role Role { get; private set; } = null!;
        public virtual EmployeeProfile? Profile { get; private set; }

        private readonly List<UserRefreshToken> _refreshTokens = new();
        public virtual IReadOnlyCollection<UserRefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public void AddRefreshToken(string token, string jwtId, TimeProvider timeProvider, DateTimeOffset expiresAt)
        {
            _refreshTokens.Add(new UserRefreshToken(this.Id, token, jwtId, timeProvider.GetUtcNow(), expiresAt));
        }

        public void RevokeAllTokens()
        {
            foreach (var token in _refreshTokens.Where(t => !t.IsRevoked))
            {
                token.Revoke();
            }
        }

        public void RecordFailedLogin(TimeProvider timeProvider, int maxAttempts, TimeSpan lockoutDuration)
        {
            FailedLoginAttempts++;
            if (FailedLoginAttempts >= maxAttempts)
            {
                LockoutEndDate = timeProvider.GetUtcNow().Add(lockoutDuration);
            }
        }

        public void ResetFailedLogins(TimeProvider timeProvider)
        {
            FailedLoginAttempts = 0;
            LockoutEndDate = null;
            LastLoginDate = timeProvider.GetUtcNow();
        }

        public void ResetPasswordByAdmin(string newPasswordHash)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newPasswordHash);

            PasswordHash = newPasswordHash;
            SecurityStamp = Guid.NewGuid().ToString();
            IsFirstLogin = true;

            RevokeAllTokens();
        }

        public void ChangePassword(string newPasswordHash)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newPasswordHash);

            PasswordHash = newPasswordHash;
            SecurityStamp = Guid.NewGuid().ToString();

            IsFirstLogin = false;

            RevokeAllTokens();
        }

        public void Deactivate()
        {
            IsActive = false;
            RevokeAllTokens();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void ChangeRole(UserRole newRole)
        {
            RoleId = (long)newRole;
        }

        public void UpdateLastLogin(TimeProvider timeProvider)
        {
            LastLoginDate = timeProvider.GetUtcNow();
        }
    }
}
