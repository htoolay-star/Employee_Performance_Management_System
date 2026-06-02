using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Entities.Auth
{
    public class PositionRole : AuditableEntity, ISoftDeletable
    {
        private PositionRole() { }

        public PositionRole(long positionId, long roleId)
        {
            if (positionId <= 0) throw new ArgumentException("Invalid Position Id.");
            if (roleId <= 0) throw new ArgumentException("Invalid Role Id.");

            PositionId = positionId;
            RoleId = roleId;
            IsActive = true;
        }

        public long PositionId { get; private set; }
        public long RoleId { get; private set; }
        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Position Position { get; private set; } = null!;
        public virtual Role Role { get; private set; } = null!;

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}