using EPMS.Domain.Contracts;

namespace EPMS.Domain.Entities.EmployeeInfo
{
    public class EmployeePayrollInfo : AuditableEntity, ISoftDeletable
    {
        private EmployeePayrollInfo() { }

        public EmployeePayrollInfo(long employeeId, decimal salary, string? currency)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(salary);

            EmployeeId = employeeId;
            Salary = salary;
            Currency = currency?.Trim().ToUpperInvariant();
        }

        public long EmployeeId { get; private set; }
        public decimal Salary { get; private set; }
        public string? Currency { get; private set; }
        public string? PayType { get; private set; }
        public DateOnly? DateOfPayTypeChanged { get; private set; }

        public string? CostAllocate { get; private set; }
        public string? PayByBacklog { get; private set; }

        public string? TaxStatus { get; private set; }
        public string? TaxNo { get; private set; }
        public string? SSBStatus { get; private set; }
        public string? SSCBNo { get; private set; }
        public int? ComplianceEarnedPoints { get; private set; }
        public int? ComplianceBalancePoints { get; private set; }

        public DateOnly? DateOfSalaryChanged { get; private set; }
        public DateOnly? DateOfCurrencyChange { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public byte[] Version { get; private set; } = Array.Empty<byte>();

        public virtual EmployeeProfile Profile { get; private set; } = null!;
        public void UpdatePayrollDetails(decimal salary, string? costAllocate, string? payByBacklog)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(salary);

            Salary = salary;
            CostAllocate = costAllocate?.Trim();
            PayByBacklog = payByBacklog?.Trim();
        }

        public void UpdateSalary(decimal newSalary, string payType, DateOnly changeDate, string? currency = null, DateOnly? currencyChangeDate = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(newSalary);
            ArgumentException.ThrowIfNullOrWhiteSpace(payType);

            Salary = newSalary;

            if (payType.Trim() != PayType)
            {
                PayType = payType.Trim();
                DateOfPayTypeChanged = changeDate;
            }

            DateOfSalaryChanged = changeDate;

            if (currency != null)
            {
                var sanitizedCurrency = currency.Trim().ToUpperInvariant();
                if (sanitizedCurrency != Currency)
                {
                    Currency = sanitizedCurrency;
                    DateOfCurrencyChange = currencyChangeDate ?? changeDate;
                }
            }
        }

        public void UpdateTaxInfo(string? taxStatus, string? taxNo)
        {
            TaxStatus = taxStatus?.Trim();
            TaxNo = taxNo?.Trim();
        }

        public void UpdateSSBInfo(string? ssbStatus, string? sscbNo)
        {
            SSBStatus = ssbStatus?.Trim();
            SSCBNo = sscbNo?.Trim();
        }

        public void UpdateCompliancePoints(int? earnedPoints, int? balancePoints)
        {
            ComplianceEarnedPoints = earnedPoints;
            ComplianceBalancePoints = balancePoints;
        }
    }
}
