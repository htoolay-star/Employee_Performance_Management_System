using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using EPMS.Domain.Entities.Hr;
using System;

namespace EPMS.Domain.Entities.EmployeeInfo
{
    public class EmployeeEmploymentHistory : BaseEntity
    {
        private EmployeeEmploymentHistory() { }

        public EmployeeEmploymentHistory(
            long employeeId,
            long departmentId,
            long positionId,
            long? managerId,
            string employmentStatus,
            DateOnly effectiveDate,
            string? changeReason = null,
            long? changedById = null)
        {
            EmployeeId = employeeId;
            DepartmentId = departmentId;
            PositionId = positionId;
            ManagerId = managerId;
            EmploymentStatus = employmentStatus?.Trim() ?? string.Empty;
            EffectiveDate = effectiveDate;
            ChangeReason = changeReason?.Trim();
            ChangedById = changedById;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public long EmployeeId { get; private set; }
        public long DepartmentId { get; private set; }
        public long PositionId { get; private set; }
        public long? ManagerId { get; private set; }

        public string EmploymentStatus { get; private set; } = string.Empty;
        public DateOnly EffectiveDate { get; private set; }
        public DateOnly? EndDate { get; private set; }

        public string? ChangeReason { get; private set; }
        public long? ChangedById { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public virtual EmployeeProfile Employee { get; private set; } = null!;
        public virtual Department Department { get; private set; } = null!;
        public virtual Position Position { get; private set; } = null!;
        public virtual EmployeeProfile? Manager { get; private set; }
        public virtual User? ChangedBy { get; private set; }

        public void EndEmploymentPeriod(DateOnly endDate)
        {
            if (endDate < EffectiveDate)
                throw new ArgumentException("End date cannot be before effective date.");

            EndDate = endDate;
        }
    }
}
