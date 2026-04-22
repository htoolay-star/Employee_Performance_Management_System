using EPMS.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPMS.Domain.Entities.Hr
{
    public class Team : IAuditableEntity
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

        public long Id { get; private set; }
        public string Name { get; private set; } = string.Empty;


        public long DepartmentId { get; private set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; private set; } = null!;

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
    }
}