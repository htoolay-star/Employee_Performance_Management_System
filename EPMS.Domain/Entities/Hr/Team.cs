using EPMS.Domain.Contracts;
using System;

namespace EPMS.Domain.Entities.Hr
{
    public class Team : AuditableEntity , ISoftDeletable
    {

        private Team() { }

        public Team(string name, long departmentId)
        {

            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            if (departmentId <= 0) throw new ArgumentException("Invalid Department Id.");

            Name = name.Trim();
            DepartmentId = departmentId;
            IsActive = true;
        }

        public string Name { get; private set; } = string.Empty;

        public long DepartmentId { get; private set; }
        public virtual Department Department { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);
            Name = newName.Trim();
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}