using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.PerformanceDTOs.PositionKPIDTOs
{
    public class PositionKPIDto
    {
        public long PositionId { get; init; }
        public string PositionTitle { get; init; } = string.Empty;
        public long KPIId { get; init; }
        public string KPIName { get; init; } = string.Empty;
        public long PriorityId { get; init; }
        public string PriorityLevelName { get; init; } = string.Empty;
        public decimal Weightage { get; init; }
        public string? TargetValue { get; init; }
        public string? TargetUnit { get; init; }
    }
}
