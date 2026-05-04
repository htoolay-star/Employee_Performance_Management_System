using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Auth
{
    public class UserRefreshToken : BaseEntity
    {
        private UserRefreshToken() { }

        public UserRefreshToken(long userId, string token, string jwtId, DateTimeOffset createdAt, DateTimeOffset expiresAt)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token);
            ArgumentException.ThrowIfNullOrWhiteSpace(jwtId);

            UserId = userId;
            Token = token;
            JwtId = jwtId;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
        }

        public long UserId { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public string JwtId { get; private set; } = string.Empty;

        public bool IsUsed { get; private set; }
        public bool IsRevoked { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();
        public virtual User User { get; private set; } = null!;

        public bool IsExpired(TimeProvider timeProvider) => timeProvider.GetUtcNow() >= ExpiresAt;

        public bool IsActive(TimeProvider timeProvider) => !IsUsed && !IsRevoked && !IsExpired(timeProvider);

        public void MarkAsUsed() => IsUsed = true;

        public void Revoke() => IsRevoked = true;
    }
}
