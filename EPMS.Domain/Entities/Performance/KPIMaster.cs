using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Performance
{
    public class KPIMaster : AuditableEntity , ISoftDeletable
    {
        private KPIMaster() { }

        public KPIMaster(int categoryId, string code, string name, string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            CategoryId = categoryId;
            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description?.Trim();
            IsActive = true;
        }

        public int CategoryId { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Category Category { get; private set; } = null!;
    }
}
