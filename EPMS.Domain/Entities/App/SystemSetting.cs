using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.App
{
    public class SystemSetting : IAuditableEntity , ISoftDeletable
    {
        private SystemSetting() { }

        public SystemSetting(string key, string value, string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            Key = key.Trim();
            Value = value.Trim();
            Description = description?.Trim();
        }

        public int Id { get; private set; }
        public string Key { get; private set; } = string.Empty;
        public string Value { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public void UpdateValue(string newValue)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newValue);
            Value = newValue.Trim();
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription?.Trim();
        }
    }
}
