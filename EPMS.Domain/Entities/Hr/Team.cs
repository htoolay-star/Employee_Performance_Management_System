using EPMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.Hr
{
    public class Team : IAuditableEntity
    {
        private Team() { }
        public Team(string name, long departmentId)
        {
            Name = name;
            DepartmentId = departmentId;
            IsActive = true;
        }

        public long Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public long DepartmentId { get; private set; }
        public virtual Department Department { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public void Rename(string newName) => Name = newName;
        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}
