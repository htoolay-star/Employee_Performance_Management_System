using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Shared.DTOs.PerformanceImprovementPlan
{
    public class AddPIPProgressDto
    {
        public Guid PIPId { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public string ProgressStatus { get; set; } = string.Empty;
    }
}
