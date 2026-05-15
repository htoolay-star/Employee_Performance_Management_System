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
    public class Position : AuditableEntity , ISoftDeletable
    {
        private Position() { }

        public Position(string code, string name, long levelId, string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description;
            LevelId = levelId;
            IsActive = true;
        }

        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public long LevelId { get; private set; }
        public virtual Level Level { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;

        public void Update(string code, string name, long levelId, string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            LevelId = levelId;
        }

        public void SetDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }

        private readonly List<PositionPermission> _positionPermissions = new();
        public virtual IReadOnlyCollection<PositionPermission> PositionPermissions => _positionPermissions.AsReadOnly();

        private readonly List<PositionRole> _positionRoles = new();
        public virtual IReadOnlyCollection<PositionRole> PositionRoles => _positionRoles.AsReadOnly();

        private readonly List<PositionFormTemplate> _positionFormTemplates = new();
        public virtual IReadOnlyCollection<PositionFormTemplate> PositionFormTemplates => _positionFormTemplates.AsReadOnly();

        private readonly List<PositionPIPTemplate> _positionPIPTemplates = new();
        public virtual IReadOnlyCollection<PositionPIPTemplate> PositionPIPTemplates => _positionPIPTemplates.AsReadOnly();

        public void AssignPermission(long permissionId)
        {
            if (!_positionPermissions.Any(p => p.PermissionId == permissionId))
            {
                _positionPermissions.Add(new PositionPermission(Id, permissionId));
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
