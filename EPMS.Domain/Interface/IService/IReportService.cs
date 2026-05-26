using EPMS.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Interface.IService
{
    public interface IReportService
    {
        Task<IEnumerable<EmployeePerformanceSummaryDto>> GetEmployeePerformanceSummaryAsync();
    }
}
