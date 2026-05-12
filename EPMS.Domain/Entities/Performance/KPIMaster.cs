using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;

namespace EPMS.Domain.Entities.Performance
{
    public class KPIMaster : IAuditableEntity
    {
        private KPIMaster() { }

        public KPIMaster(
            long categoryId,
            string code,
            string name,
            string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            CategoryId = categoryId;
            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description?.Trim();

            IsActive = true;
        }

        public long Id { get; private set; }

        public long CategoryId { get; private set; }

        public string Code { get; private set; } = string.Empty;

        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Category Category { get; private set; } = null!;

        public void Update(
            long categoryId,
            string code,
            string name,
            string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            CategoryId = categoryId;
            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description?.Trim();
        }

        public void Deactivate() => IsActive = false;

        public void Reactivate() => IsActive = true;

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}