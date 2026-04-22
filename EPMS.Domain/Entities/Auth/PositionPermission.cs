using EPMS.Domain.Entities.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Auth
{
    public class PositionPermission
    {
        private PositionPermission() { }

        public PositionPermission(long positionId, int permissionId)
        {
            PositionId = positionId;
            PermissionId = permissionId;
        }

        public long PositionId { get; private set; }
        public int PermissionId { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public virtual Permission Permission { get; private set; } = null!;
        public virtual Position Position { get; private set; } = null!;
    }
}
