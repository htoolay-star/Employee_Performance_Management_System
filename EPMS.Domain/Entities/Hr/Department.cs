using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;

namespace EPMS.Domain.Entities.Hr
{
    public class Department : AuditableEntity, ISoftDeletable
    {
        private Department() { }

        public Department(string code, string name, string? description = null, long? deptHeadId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description;
            DeptHeadId = deptHeadId;
            IsActive = true;
        }

        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public long? DeptHeadId { get; private set; }
        public virtual EmployeeProfile? DeptHead { get; private set; }

        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);
            Name = newName.Trim();
        }

        public void SetDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }

        public void SetDeptHead(long? deptHeadId)
        {
            DeptHeadId = deptHeadId;
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
        private readonly List<Team> _teams = new();
        public virtual IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();

        public void AddTeam(string code, string teamName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(teamName);
            var sanitizedCode = code.Trim().ToUpperInvariant();
            var sanitizedName = teamName.Trim();

            if (_teams.Any(t => t.Name.Equals(sanitizedName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Team with name '{sanitizedName}' already exists in this department.");
            }

            var newTeam = new Team(sanitizedCode, sanitizedName, this.Id);
            _teams.Add(newTeam);
        }
    }
}
