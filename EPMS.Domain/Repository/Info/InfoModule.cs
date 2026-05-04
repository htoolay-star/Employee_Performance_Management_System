using EPMS.Domain.Data;
using EPMS.Domain.Entities.App;
using EPMS.Domain.Interface.Irepo.App;
using EPMS.Domain.Interface.Irepo.Info;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Repository.Info
{
    public class InfoModule(IServiceProvider serviceProvider) : IInfoModule
    {
        private IEmployeeProfileRepository? _profiles;
        private IEmployeeContactRepository? _contacts;
        private IEmployeeEmploymentRepository? _employments;
        private IEmployeeEmploymentHistoryRepository? _employmentHistories;
        private IEmployeeFamilyInfoRepository? _familyInfos;
        private IEmployeePayrollInfoRepository? _payrollInfos;
        private IEmployeeSalaryHistoryRepository? _salaryHistories;

        public IEmployeeProfileRepository EmployeeProfiles =>
            _profiles ??= serviceProvider.GetRequiredService<IEmployeeProfileRepository>();

        public IEmployeeContactRepository EmployeeContacts =>
            _contacts ??= serviceProvider.GetRequiredService<IEmployeeContactRepository>();

        public IEmployeeEmploymentRepository EmployeeEmployments =>
            _employments ??= serviceProvider.GetRequiredService<IEmployeeEmploymentRepository>();

        public IEmployeeEmploymentHistoryRepository EmployeeEmploymentHistories =>
            _employmentHistories ??= serviceProvider.GetRequiredService<IEmployeeEmploymentHistoryRepository>();

        public IEmployeeFamilyInfoRepository EmployeeFamilyInfos =>
            _familyInfos ??= serviceProvider.GetRequiredService<IEmployeeFamilyInfoRepository>();

        public IEmployeePayrollInfoRepository EmployeePayrollInfos =>
            _payrollInfos ??= serviceProvider.GetRequiredService<IEmployeePayrollInfoRepository>();

        public IEmployeeSalaryHistoryRepository EmployeeSalaryHistories =>
            _salaryHistories ??= serviceProvider.GetRequiredService<IEmployeeSalaryHistoryRepository>();
    }
}
