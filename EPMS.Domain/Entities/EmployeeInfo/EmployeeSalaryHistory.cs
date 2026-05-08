using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Auth;
using System;

namespace EPMS.Domain.Entities.EmployeeInfo
{
    public class EmployeeSalaryHistory : BaseEntity
    {
        private EmployeeSalaryHistory() { }

        public EmployeeSalaryHistory(
            long employeeId,
            decimal previousAmount,
            decimal newAmount,
            DateOnly effectiveDate,
            string changeReason,
            TimeProvider timeProvider,
            long? approvedById = null)
        {
            if (previousAmount < 0 || newAmount < 0)
                throw new ArgumentException("Salary amounts cannot be negative.");

            ArgumentException.ThrowIfNullOrWhiteSpace(changeReason);

            EmployeeId = employeeId;
            PreviousAmount = previousAmount;
            NewAmount = newAmount;
            EffectiveDate = effectiveDate;
            ChangeReason = changeReason.Trim();
            ApprovedById = approvedById;
            ApprovedAt = approvedById.HasValue ? timeProvider.GetUtcNow() : null;
            CreatedAt = timeProvider.GetUtcNow();
        }

        public long EmployeeId { get; private set; }

        public decimal PreviousAmount { get; private set; }
        public decimal NewAmount { get; private set; }
        public decimal PercentageChange => PreviousAmount == 0 ? 0 : ((NewAmount - PreviousAmount) / PreviousAmount) * 100;

        public DateOnly EffectiveDate { get; private set; }
        public string ChangeReason { get; private set; } = string.Empty;

        public long? ApprovedById { get; private set; }
        public DateTimeOffset? ApprovedAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public virtual EmployeeProfile Employee { get; private set; } = null!;
        public virtual User? ApprovedBy { get; private set; }

        public void Approve(long approvedById, TimeProvider timeProvider)
        {
            ApprovedById = approvedById;
            ApprovedAt = timeProvider.GetUtcNow();
        }
    }
}
