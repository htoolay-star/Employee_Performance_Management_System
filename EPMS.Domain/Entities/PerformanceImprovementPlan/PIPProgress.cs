using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Entities.PerformanceImprovementPlan
{
    public class PIPProgress
    {
        public Guid Id { get; set; }

        public Guid PIPId { get; set; }

        public string Feedback { get; set; } = string.Empty;

        public DateTime ReviewDate { get; set; }

        public string ProgressStatus { get; set; } = string.Empty;
        // Improving / No Change / Worsening

        public PerformanceImprovementPlan PIP { get; set; } = null!;
    }
}
