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
    public class Position : IAuditableEntity , ISoftDeletable
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

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;

        public void Update(string title, int levelId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            Title = title.Trim();
            LevelId = levelId;
        }

        private readonly List<PositionPermission> _positionPermissions = new();
        public virtual IReadOnlyCollection<PositionPermission> PositionPermissions => _positionPermissions.AsReadOnly();

        private readonly List<PositionKPI> _positionKPIs = new();
        public virtual IReadOnlyCollection<PositionKPI> PositionKPIs => _positionKPIs.AsReadOnly();

        private readonly List<PositionFormTemplate> _positionFormTemplates = new();
        public virtual IReadOnlyCollection<PositionFormTemplate> PositionFormTemplates => _positionFormTemplates.AsReadOnly();

        private readonly List<PositionPIPTemplate> _positionPIPTemplates = new();
        public virtual IReadOnlyCollection<PositionPIPTemplate> PositionPIPTemplates => _positionPIPTemplates.AsReadOnly();

        public void AssignPermission(int permissionId)
        {
            if (!_positionPermissions.Any(p => p.PermissionId == permissionId))
            {
                _positionPermissions.Add(new PositionPermission(this.Id, permissionId));
            }
        }

        public void AddFormTemplate(PositionFormTemplate template)
        {
            ArgumentNullException.ThrowIfNull(template);

            if (_positionFormTemplates.Any(t => t.FormTemplateId == template.FormTemplateId))
            {
                throw new InvalidOperationException("This form template is already assigned to this position.");
            }

            _positionFormTemplates.Add(template);
        }

        public void RemoveFormTemplate(PositionFormTemplate template)
        {
            ArgumentNullException.ThrowIfNull(template);
            _positionFormTemplates.Remove(template);
        }

        public void AddPIPTemplate(PositionPIPTemplate template)
        {
            ArgumentNullException.ThrowIfNull(template);

            if (_positionPIPTemplates.Any(t => t.Title.Equals(template.Title, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A PIP template with the title '{template.Title}' already exists for this position.");
            }

            _positionPIPTemplates.Add(template);
        }

        public void RemovePIPTemplate(PositionPIPTemplate template)
        {
            ArgumentNullException.ThrowIfNull(template);
            _positionPIPTemplates.Remove(template);
        }
    }
}
