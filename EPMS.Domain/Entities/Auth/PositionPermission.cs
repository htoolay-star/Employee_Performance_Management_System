using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;

namespace EPMS.Domain.Entities.Auth
{
    public class PositionPermission : AuditableEntity, ISoftDeletable
    {
        private PositionPermission() { }

        public PositionPermission(long positionId, long permissionId)
        {
            PositionId = positionId;
            PermissionId = permissionId;
        }

        public long PositionId { get; private set; }
        public long PermissionId { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Permission Permission { get; private set; } = null!;
        public virtual Position Position { get; private set; } = null!;
    }
}
