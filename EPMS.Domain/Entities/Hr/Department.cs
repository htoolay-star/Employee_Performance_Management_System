using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Hr
{
    public class Department : IAuditableEntity , ISoftDeletable
    {
        private Department() { }

        public Department(string code, string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();

            IsActive = true;
        }

        public long Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;

        public bool IsActive { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);
            Name = newName.Trim();
        }
        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
        private readonly List<Team> _teams = new();
        public virtual IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();

        public void AddTeam(string teamName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(teamName);
            var sanitizedName = teamName.Trim();

            if (_teams.Any(t => t.Name.Equals(sanitizedName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Team with name '{sanitizedName}' already exists in this department.");
            }

            var newTeam = new Team(sanitizedName, this.Id);
            _teams.Add(newTeam);
        }
    }
}
