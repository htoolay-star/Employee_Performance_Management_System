using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.KPI
{
    public class CreatePositionKPIDto
    {
        public long PositionId { get; set; }

        public long KPIId { get; set; }

        public long PriorityId { get; set; }

        public decimal Weightage { get; set; }

        public string? TargetValue { get; set; }

        public string? TargetUnit { get; set; }
    }
}
