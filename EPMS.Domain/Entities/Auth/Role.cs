using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Auth
{
    public class Role : AuditableEntity , ISoftDeletable
    {
        private Role() { }

        public Role(long id, string name, string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Id = id;
            Name = name.Trim();
            Description = description?.Trim();
        }

        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public void UpdateDetails(string name, string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name.Trim();
            Description = description?.Trim();
        }
    }
}
