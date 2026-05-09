using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.PerformanceImprovementPlan
{
    public class CreatePIPDto
    {
        public Guid EmployeeId { get; set; }
        public string Objectives { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; }
    }
}
