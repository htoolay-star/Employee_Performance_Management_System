using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual Permission Permission { get; private set; } = null!;
        public virtual Position Position { get; private set; } = null!;
    }
}
