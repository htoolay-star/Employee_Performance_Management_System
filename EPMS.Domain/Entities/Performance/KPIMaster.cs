using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Shared;
using EPMS.Shared.Constants;

namespace EPMS.Domain.Entities.Performance
{
    public class KPIMaster : AuditableEntity, ISoftDeletable
    {
        private KPIMaster() { }

        public KPIMaster(long categoryId, string code, string name, string? description = null, string? scoringDirection = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            CategoryId = categoryId;
            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description?.Trim();
            ScoringDirection = scoringDirection ?? AppraisalConstants.ScoringDirections.HigherIsBetter;
            IsActive = true;
        }

        public long CategoryId { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public string ScoringDirection { get; private set; } = AppraisalConstants.ScoringDirections.HigherIsBetter;
        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Category Category { get; private set; } = null!;

        public void Update(long categoryId, string code, string name, string? description, string? scoringDirection = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            CategoryId = categoryId;
            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description?.Trim();
            if (scoringDirection != null)
                ScoringDirection = scoringDirection;
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
