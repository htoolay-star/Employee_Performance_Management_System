using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.KPI
{
    public class PositionKPIDto
    {
        public long Id { get; set; }

        public string KPIName { get; set; } = string.Empty;

        public string PriorityName { get; set; } = string.Empty;

        public decimal Weightage { get; set; }

        public string? TargetValue { get; set; }

        public string? TargetUnit { get; set; }
    }
}
