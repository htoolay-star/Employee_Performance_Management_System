using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Shared
{
    public class Category : AuditableEntity , ISoftDeletable
    {
        private Category() { }

        public Category(string module, string code, string name, string? description = null, int? parentId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(module);
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Module = module.Trim().ToUpperInvariant();
            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description?.Trim();

            ParentId = parentId;
            IsActive = true;
        }

        public string Module { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public int? ParentId { get; private set; }
        public virtual Category? Parent { get; private set; }
        public virtual ICollection<Category> SubCategories { get; private set; } = new List<Category>();

        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public void UpdateDetails(string name, string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
            Description = description?.Trim();
        }

        public void MoveToParent(int? newParentId)
        {
            if (newParentId.HasValue && newParentId.Value == this.Id)
            {
                throw new InvalidOperationException("A category cannot be its own parent.");
            }

            ParentId = newParentId;
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
