using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.KPI
{
    public class SubmitEmployeeKPIDto
    {
        public long EmployeeId { get; set; }

        public long PositionKPIId { get; set; }

        public long PerformanceCycleId { get; set; }

        public decimal TargetValue { get; set; }

        public decimal ActualValue { get; set; }

        public bool IsNegativeKPI { get; set; }
    }
}
