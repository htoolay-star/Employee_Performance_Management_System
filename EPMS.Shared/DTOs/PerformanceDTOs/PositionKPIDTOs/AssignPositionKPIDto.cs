using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.PerformanceDTOs.PositionKPIDTOs
{
    public class AssignPositionKPIDto
    {
        public long PositionId { get; init; }
        public long KPIId { get; init; }
        public long PriorityId { get; init; }
        public decimal Weightage { get; init; }
        public string? TargetValue { get; init; }
        public string? TargetUnit { get; init; }
        public string? ChangeReason { get; init; } = string.Empty;
    }
}
