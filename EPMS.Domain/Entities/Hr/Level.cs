using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Hr
{
    public class Level : IAuditableEntity , ISoftDeletable
    {
        private Level() { }

        public Level(string code, string name, string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();

            Description = description;
            IsActive = true;
        }

        public int Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;

        public void Update(string name, string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }
    }
}
