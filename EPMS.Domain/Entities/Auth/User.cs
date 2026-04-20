using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Auth
{
    public class User : IAuditableEntity
    {
        private User() { }

        public User(string email, string passwordHash)
        {

            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

            Email = email;
            PasswordHash = passwordHash;
            SecurityStamp = Guid.NewGuid().ToString();
            IsFirstLogin = true;
            IsActive = true;
            FailedLoginAttempts = 0;
        }

        public long Id { get; private set; }
        public Guid UserGuid { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string NormalizedEmail { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string SecurityStamp { get; private set; } = string.Empty;

        public bool IsFirstLogin { get; private set; }
        public bool IsActive { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTimeOffset? LockoutEndDate { get; private set; }
        public DateTimeOffset? LastLoginDate { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public byte[] Version { get; private set; } = Array.Empty<byte>();

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

        public void ChangePassword(string newPasswordHash)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newPasswordHash);

            PasswordHash = newPasswordHash;
            SecurityStamp = Guid.NewGuid().ToString();
            IsFirstLogin = false;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
