using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Shared
{
    public class Tag : BaseEntity
    {
        private Tag() { }

        public Tag(string name, string? module = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim().ToLowerInvariant();
            Module = module?.Trim().ToUpperInvariant();
        }

        public string Name { get; private set; } = string.Empty;
        public string? Module { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
    }
}
