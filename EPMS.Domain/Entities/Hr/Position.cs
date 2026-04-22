using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Hr
{
    public class Position : IAuditableEntity
    {
        private Position() { }

        public Position(string title, int levelId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);

            Title = title.Trim();

            LevelId = levelId;
            IsActive = true;
        }

        public long Id { get; private set; }
        public string Title { get; private set; } = string.Empty;

        public int LevelId { get; private set; }
        public virtual Level Level { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;

        private readonly List<PositionPermission> _positionPermissions = new();
        public virtual IReadOnlyCollection<PositionPermission> PositionPermissions => _positionPermissions.AsReadOnly();

        private readonly List<PositionKPI> _positionKPIs = new();
        public virtual IReadOnlyCollection<PositionKPI> PositionKPIs => _positionKPIs.AsReadOnly();

        public void AssignPermission(int permissionId)
        {
            if (!_positionPermissions.Any(p => p.PermissionId == permissionId))
            {
                _positionPermissions.Add(new PositionPermission(this.Id, permissionId));
            }
        }
    }
}
