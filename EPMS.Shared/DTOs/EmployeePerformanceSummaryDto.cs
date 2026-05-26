using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs
{
    public class EmployeePerformanceSummaryDto
    {
        // EmployeeProfile မှ လာမည့် အချက်အလက်များ
        public string StaffNo { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;

        // EmployeeEmployment မှ လာမည့် အချက်အလက်များ
        public string DepartmentName { get; set; } = string.Empty;
        public string PositionTitle { get; set; } = string.Empty;
        public string EmploymentStatus { get; set; } = string.Empty;

        // Performance (EmployeeKPIs) မှ တွက်ချက်ရမည့် အချက်အလက်များ
        public double TotalKPIWeight { get; set; }
        public double FinalAppraisalScore { get; set; }
        public string PerformanceGrade { get; set; } = "Not Evaluated";
    }
}
