using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Hr
{
    public class Department : IAuditableEntity
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

        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);
            Name = newName.Trim();
        }
        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
        public void AddTeam(string teamName)
        {
            if (Teams.Any(t => t.Name.Equals(teamName.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Team with name '{teamName}' already exists in this department.");
            }

            Teams.Add(new Team(teamName, this.Id));
        }
    }
}
