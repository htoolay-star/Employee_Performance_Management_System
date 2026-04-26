using EPMS.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.App
{
    public class Notification
    {
        private Notification() { }

        public Notification(long toUserId, string title, string message, string type, string? url = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);
            ArgumentException.ThrowIfNullOrWhiteSpace(type);

            ToUserId = toUserId;

            Title = title.Trim();
            Message = message.Trim();
            Type = type.Trim().ToUpperInvariant();
            RedirectUrl = url?.Trim();

            IsRead = false;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public long Id { get; private set; }
        public long ToUserId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Message { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;
        public string? RedirectUrl { get; private set; }

        public bool IsRead { get; private set; }
        public DateTimeOffset? ReadAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual User User { get; private set; } = null!;

        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                ReadAt = DateTimeOffset.UtcNow;
            }
        }
    }
}
