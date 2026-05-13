namespace EPMS.Client.Models.Info
{
    public class EmployeePayrollFormModel
    {
        public long Id { get; set; }
        public decimal Salary { get; set; }
        public string? Currency { get; set; }
        public string? PayType { get; set; }
        public string? CostAllocate { get; set; }
        public string? PayByBacklog { get; set; }
        public string? TaxStatus { get; set; }
        public string? TaxNo { get; set; }
        public string? SSBStatus { get; set; }
        public string? SSCBNo { get; set; }
        public int? ComplianceEarnedPoints { get; set; }
        public int? ComplianceBalancePoints { get; set; }
    }
}