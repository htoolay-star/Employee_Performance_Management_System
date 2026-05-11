using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.EmployeeInfo;
using System;

namespace EPMS.Domain.Entities.Hr
{
    public class Team : AuditableEntity , ISoftDeletable
    {

        private Team() { }

        public Team(string code, string name, long departmentId, string? description = null, long? leadTeamId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            if (departmentId <= 0) throw new ArgumentException("Invalid Department Id.");

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = description;
            LeadTeamId = leadTeamId;
            DepartmentId = departmentId;
            IsActive = true;
        }

        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public long? LeadTeamId { get; private set; }
        public virtual EmployeeProfile? LeadTeam { get; private set; }

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

        public void Update(string code, string name, string? description, long? leadTeamId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            LeadTeamId = leadTeamId;
        }

        public void SetCode(string code)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            Code = code.Trim().ToUpperInvariant();
        }

        public void SetDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }

        public void SetLeadTeam(long? leadTeamId)
        {
            LeadTeamId = leadTeamId;
        }

        public void ReassignToDepartment(long newDepartmentId)
        {
            if (newDepartmentId <= 0)
                throw new ArgumentException("Invalid Department Id.");
            DepartmentId = newDepartmentId;
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
    }
}