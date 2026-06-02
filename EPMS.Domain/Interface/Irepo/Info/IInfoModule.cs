namespace EPMS.Domain.Interface.Irepo.Info
{
    public interface IInfoModule
    {
        IEmployeeProfileRepository EmployeeProfiles { get; }
        IEmployeeContactRepository EmployeeContacts { get; }
        IEmployeeEmploymentRepository EmployeeEmployments { get; }
        IEmployeeEmploymentHistoryRepository EmployeeEmploymentHistories { get; }
        IEmployeeFamilyInfoRepository EmployeeFamilyInfos { get; }
        IEmployeePayrollInfoRepository EmployeePayrollInfos { get; }
        IEmployeeSalaryHistoryRepository EmployeeSalaryHistories { get; }
    }
}
